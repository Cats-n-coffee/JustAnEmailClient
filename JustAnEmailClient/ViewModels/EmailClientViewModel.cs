using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustAnEmailClient.Views;
using JustAnEmailClient.Services;
using JustAnEmailClient.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;
using MailKit;
using MailKit.Search;

namespace JustAnEmailClient.ViewModels;

public partial class EmailClientViewModel : ObservableObject
{
    public ObservableCollection<EmailReceived> emailsReceived = new ObservableCollection<EmailReceived>();
    public ObservableCollection<EmailReceived> EmailsReceived
    {
        get => emailsReceived;
        set => SetProperty(ref emailsReceived, value);
    }

    string selectedMessageId = "";
    IMailFolder messageFolder = null; // This should be a wrapper around all the messages, will move

    string selectedMessageSender = "";

    EmailReceived selectedEmail = null;

    [ObservableProperty]
    bool messageDisplayIsVisible = false;
    [ObservableProperty]
    string textBody = "";
    [ObservableProperty]
    string htmlBody = "";
    [ObservableProperty]
    string markAsText = "";
 
    [RelayCommand]
    void OpenNewMessage()
    {
        Debug.WriteLine(Application.Current.Windows.Count);
        Window newMessageWindow = new Window(new NewMessagePage());
        Application.Current.OpenWindow(newMessageWindow);
    }

    [RelayCommand]
    void FetchPop()
    {
        string creds = FileSystemOperations.ReadTextFileSync("creds.txt");
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        List<EmailReceived> allEmails = MailReceiver.ReceiveEmailImap4(splitCreds[0], splitCreds[1]);
        EmailsReceived = new ObservableCollection<EmailReceived>(allEmails);
    }

    [RelayCommand]
    void SelectMessage(EmailReceived msg)
    {
        selectedEmail = msg;
        TextBody = msg.BodyAsText;
        HtmlBody = msg.BodyAsHtml;
        selectedMessageId = msg.MessageId;
        messageFolder = msg.MessageFolder;
        selectedMessageSender = msg.Sender;
        MarkAsText = ChooseMarkAsReadText(msg.MarkAsReadIcon);

        MessageDisplayIsVisible = true;
    }

    [RelayCommand]
    void Reply()
    {
        Debug.WriteLine($"REPLY: {selectedMessageSender}");
    }

    [RelayCommand]
    void DeleteMessage() 
    {
        var uid = messageFolder.Search(SearchQuery.HeaderContains("Message-Id", selectedMessageId));
        messageFolder.AddFlags(uid, MessageFlags.Deleted, true);
    }

    [RelayCommand]
    void ForwardMessage()
    {

    }

    [RelayCommand]
    void ToggleMarkAsRead()
    {
        // Update UI
        selectedEmail.MarkAsReadIcon = !selectedEmail.MarkAsReadIcon;
        MarkAsText = ChooseMarkAsReadText(selectedEmail.MarkAsReadIcon);

        // Boolean switch to match naming, since the icon shows when email is unread
        MailReceiver.MarkOrUnmarkAsRead(
            selectedEmail.MessageFolder,
            selectedEmail.MessageId,
            !selectedEmail.MarkAsReadIcon
        );
    }

    // Helper Functions
    static string ChooseMarkAsReadText(bool markAsReadIconStatus)
    {
        return markAsReadIconStatus ? "Mark as Read" : "Mark as Unread";
    }
}
