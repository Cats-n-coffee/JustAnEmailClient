using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using JustAnEmailClient.Models;
using System.Diagnostics;

namespace JustAnEmailClient.Services;

public class ImapService
{
    private readonly ImapClient imapClient = null;
    public ImapService(string email, string password)
    {
        imapClient = StartImapClient(email, password);
    }

    private static ImapClient StartImapClient(string email, string password)
    {
        var client = new ImapClient();
        client.Connect("outlook.office365.com", 993, true);
        client.Authenticate(email, password);

        return client;
    }

    public IList<IMailFolder> GetFolders()
    {
        var folders = imapClient.GetFolders(new FolderNamespace('.', "")); // use async
        // store folders inside class?
        // Make new class model for folders
        foreach (var folder in folders)
        {
            Debug.WriteLine("[folder] {0}", folder.FullName);
        }

        return folders;
    }

    public List<EmailReceived> GetFolderMessages(IMailFolder folder)
    {
        List<EmailReceived> emails = new List<EmailReceived>();

        for (int i = 0; i < folder.Count; i++)
        {
            var message = folder.GetMessage(i);
            EmailReceived emailReceived = new EmailReceived();
            emailReceived.Sender = message.From.Mailboxes.FirstOrDefault().Address;
            emailReceived.Subject = message.Subject;
            emailReceived.DateSent = message.Date.ToString();
            emailReceived.MessageId = message.MessageId;

            emailReceived.MessageFolder = folder;
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
            var info = folder.Fetch(new[] { i }, MessageSummaryItems.Flags);
            if (info[0].Flags.Value.HasFlag(MessageFlags.Seen))
            {
                emailReceived.MarkAsReadIcon = false;
            }
            else emailReceived.MarkAsReadIcon = true;

            emails.Add(emailReceived);
        }

        // Dispose at the end? if we do, we need another method to reopen connection?
        return emails;
    }

    public static List<EmailReceived> ReceiveEmailImap4(string email, string password)
    {
        var client = new ImapClient();
        client.Connect("outlook.office365.com", 993, true);
        client.Authenticate(email, password);

        var inbox = client.Inbox;
        inbox.Open(FolderAccess.ReadWrite);

        var folders = client.GetFolders(new FolderNamespace('.', "")); // use async

        foreach (var folder in folders)
        {
            Debug.WriteLine("[folder] {0}", folder.FullName);
        }

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
            }
            else emailReceived.MarkAsReadIcon = true;

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
            if (markAsRead) folder.AddFlags(uid, MessageFlags.Seen, true);
            else folder.RemoveFlags(uid, MessageFlags.Seen, true);
        }
    }

    public static void DeleteMessage(IMailFolder folder, string msgId)
    {
        var uid = folder.Search(SearchQuery.HeaderContains("Message-Id", msgId));
        folder.AddFlags(uid, MessageFlags.Deleted, true);
    }
}
