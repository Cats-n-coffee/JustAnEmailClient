using OpenPop.Pop3;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using JustAnEmailClient.Models;
using System.Diagnostics;
using MailKit.Search;

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
            emailReceived.MessageId = client.GetMessage(i).Headers.MessageId;
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
        inbox.Open(FolderAccess.ReadWrite);

        List<EmailReceived> allEmails = new List<EmailReceived>();

        for (int i = 0; i < inbox.Count; i++)
        {
            var message = inbox.GetMessage(i);
            EmailReceived emailReceived = new EmailReceived();
            emailReceived.Sender = message.From.Mailboxes.FirstOrDefault().Address;
            emailReceived.Subject = message.Subject;
            emailReceived.DateSent = message.Date.ToString();
            emailReceived.MessageId = message.MessageId;
            
            emailReceived.MessageFolder = inbox;
            // Add message ID once needed

            if (message.HtmlBody != null)
            {
                emailReceived.BodyAsHtml = message.HtmlBody;
            }

            if (message.TextBody != null)
            {
                emailReceived.BodyAsText = message.TextBody;
            }

            // Get flags
            var info = inbox.Fetch(new[] { i }, MessageSummaryItems.Flags);
            if (info[0].Flags.Value.HasFlag(MessageFlags.Seen))
            {
                emailReceived.MarkAsReadIcon = false;
            } else emailReceived.MarkAsReadIcon = true;

            allEmails.Add(emailReceived);
        }

        // client.Disconnect(true);

        return allEmails;
    }

    public static void MarkOrUnmarkAsRead(IMailFolder folder, string msgId, bool markAsRead)
    {
        var uid = folder.Search(SearchQuery.HeaderContains("Message-Id", msgId));
        if (uid != null)
        {
            if(markAsRead) folder.AddFlags(uid, MessageFlags.Seen, true); 
            else folder.RemoveFlags(uid, MessageFlags.Seen, true);
        }
    }
}
