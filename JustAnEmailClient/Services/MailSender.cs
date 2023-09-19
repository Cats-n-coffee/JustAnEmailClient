using System.Diagnostics;
using JustAnEmailClient.Models;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace JustAnEmailClient.Services;

public class MailSender
{
    // public MailSender() {}
    // Create a struct or similar to handle all the needed data from the form
    public static void SendEmail(UserInfo userInfo, MessageToSend newMessage, bool isForward = false, bool isReply = false)
    {
        // handle no subject/no destination -> or handle in the UI directly
        var smtpClient = new SmtpClient();
        smtpClient.Connect("smtp-mail.outlook.com", 587, false);
        smtpClient.Authenticate(userInfo.email, userInfo.password);

        MimeMessage message;
        if (isForward)
        {
            message = ForwardMessageOnly(newMessage);
        }
        else if (isReply)
        {
            message = ReplyMessageText(newMessage.OriginalMessage, newMessage.Sender, newMessage.MessageContent);
        }
        else
        {
            message = SendMessageTextOnly(newMessage);
        }

        smtpClient.Send(message);
        smtpClient.Disconnect(true);
    }

    public static MimeMessage SendMessageTextOnly(MessageToSend newMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(newMessage.Sender, newMessage.Sender));
        message.To.Add(new MailboxAddress(newMessage.Recipient, newMessage.Recipient));
        message.Subject = newMessage.Subject;

        message.Body = new TextPart("plain") { Text = newMessage.MessageContent };

        return message;
    }
    // https://stackoverflow.com/questions/29414995/forward-email-using-mailkit-c
    private static MimeMessage ForwardMessageOnly(MessageToSend newMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(newMessage.Sender, newMessage.Sender));
        message.To.Add(new MailboxAddress(newMessage.Recipient, newMessage.Recipient));
        message.Subject = newMessage.Subject;

        var builder = new BodyBuilder();
        builder.TextBody = newMessage.MessageContent;
        
        if (newMessage.OriginalMessage != null)
        {
            builder.HtmlBody = newMessage.OriginalMessage.HtmlBody;
            // builder.Attachments.Add(new MessagePart { Message = newMessage.OriginalMessage });
        }

        message.Body = builder.ToMessageBody();

        return message;
    }

    // Taken from Mailkit FAQ.md
    public static MimeMessage ReplyMessageText(MimeMessage message, string from, string messageContent)
    {
        var reply = new MimeMessage();
        var senderAddress = new MailboxAddress(from, from);

        reply.From.Add(senderAddress);

        // reply to the sender of the message
        if (message.ReplyTo.Count > 0)
        {
            reply.To.AddRange(message.ReplyTo);
        }
        else if (message.From.Count > 0)
        {
            reply.To.AddRange(message.From);
        }
        else if (message.Sender != null)
        {
            reply.To.Add(message.Sender);
        }

        /*
        if (replyToAll)
        {
            // include all of the other original recipients - TODO: remove ourselves from these lists
            reply.To.AddRange(message.To);
            reply.Cc.AddRange(message.Cc);
        }*/

        // set the reply subject
        if (!message.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
            reply.Subject = "Re: " + message.Subject;
        else
            reply.Subject = message.Subject;

        // construct the In-Reply-To and References headers
        if (!string.IsNullOrEmpty(message.MessageId))
        {
            reply.InReplyTo = message.MessageId;
            foreach (var id in message.References)
                reply.References.Add(id);
            reply.References.Add(message.MessageId);
        }

        // quote the original message text
        using (var quoted = new StringWriter())
        {
            using (var reader = new StringReader(messageContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    quoted.WriteLine(line);
                }
                quoted.Write("\n\r");
            }

            var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();

            quoted.WriteLine("On {0}, {1} wrote:", message.Date.ToString("f"), !string.IsNullOrEmpty(sender.Name) ? sender.Name : sender.Address);
            using (var reader = new StringReader(message.TextBody))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    quoted.Write("> ");
                    quoted.WriteLine(line);
                }
            }

            reply.Body = new TextPart("plain")
            {
                Text = quoted.ToString()
            };
        }

        return reply;
    }
}
