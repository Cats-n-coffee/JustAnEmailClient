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

    public List<EmailReceived> GetInboxMessages() 
    {
        imapClient.Inbox.Open(FolderAccess.ReadWrite);
        return GetFolderMessages(imapClient.Inbox);
    }

    public List<Folder> GetFolders()
    {
        var incomingFolders = imapClient.GetFolders(new FolderNamespace('.', "")); // use async
        List<Folder> folders = new List<Folder>();
  
        foreach (var incomingFolder in incomingFolders)
        {
            Folder folder = new Folder();
            folder.Name = incomingFolder.FullName;
            folder.Id = incomingFolder.Id;
            folder.ImageFile = NameToImage.nameToImage.GetValueOrDefault(folder.Name.ToLower(), "folder.png");
            folder.MailFolder = incomingFolder;

            folders.Add(folder);
        }

        return folders;
    }

    public List<EmailReceived> GetFolderMessages(IMailFolder folder)
    {
        Debug.WriteLine($"getting messages {folder.Name}");
        folder.Open(FolderAccess.ReadWrite);
        List<EmailReceived> emails = new List<EmailReceived>();
        Debug.WriteLine($"count {folder.Count}");
 
        for (int i = 0; i < folder.Count; i++)
        {
            var message = folder.GetMessage(i);
            EmailReceived emailReceived = new EmailReceived();
            emailReceived.Sender = message.From.Mailboxes.FirstOrDefault().Address;
            emailReceived.Subject = message.Subject;
            emailReceived.DateSent = message.Date.ToString();
            emailReceived.MessageId = message.MessageId;
            emailReceived.OriginalMessage = message;
            emailReceived.MessageFolder = folder;

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
