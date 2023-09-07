using OpenPop.Pop3;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using JustAnEmailClient.Models;
using System.Diagnostics;

namespace JustAnEmailClient.Services;

public class MailReceiver
{
    public static List<EmailReceived> ReceiveEmailPop3(string email, string password) {
        Pop3Client client = new Pop3Client();
        client.Connect("outlook.office365.com", 995, true);
        client.Authenticate(email, password, AuthenticationMethod.UsernameAndPassword);

        int messages = client.GetMessageCount();
        List<EmailReceived> allEmails = new List<EmailReceived>();

        for (int i = messages; i > 0; i--)
        {
            EmailReceived emailReceived = new EmailReceived();
            emailReceived.Sender = client.GetMessage(i).Headers.From.ToString();
            emailReceived.Subject = client.GetMessage(i).Headers.Subject;
            emailReceived.DateSent = client.GetMessage(i).Headers.DateSent.ToString();
            emailReceived.messageId = client.GetMessage(i).Headers.MessageId;
            emailReceived.BodyAsText = client.GetMessage(i).FindFirstPlainTextVersion().GetBodyAsText();
 
            if (client.GetMessage(i).FindFirstHtmlVersion() != null)
            {
                emailReceived.BodyAsHtml = client.GetMessage(i).FindFirstHtmlVersion().GetBodyAsText();
            }

            allEmails.Add( emailReceived );
        }

        return allEmails;
    }

    public static List<EmailReceived> ReceiveEmailImap4(string email, string password)
    {
        var client = new ImapClient();
        client.Connect("outlook.office365.com", 993, true);
        client.Authenticate(email, password);

        var inbox = client.Inbox;
        inbox.Open(FolderAccess.ReadOnly); // Change this for more operations?

        List<EmailReceived> allEmails = new List<EmailReceived>();

        for (int i = 0; i < inbox.Count; i++)
        {
            var message = inbox.GetMessage(i);
            EmailReceived emailReceived = new EmailReceived();
            emailReceived.Sender = message.From.Mailboxes.FirstOrDefault().Address;
            emailReceived.Subject = message.Subject;
            emailReceived.DateSent = message.Date.ToString();
            // Add message ID once needed

            if (message.HtmlBody != null)
            {
                emailReceived.BodyAsHtml = message.HtmlBody;
            }

            if (message.TextBody != null)
            {
                emailReceived.BodyAsText = message.TextBody;
            }

            allEmails.Add(emailReceived);
        }

        client.Disconnect(true);

        return allEmails;
    }
}
