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

    EmailReceived selectedEmail = null;

    [ObservableProperty]
    bool messageDisplayIsVisible = false;
    [ObservableProperty]
    string textBody = "";
    [ObservableProperty]
    string htmlBody = "";
    [ObservableProperty]
    string markAsText = "";

    public EmailClientViewModel()
    {
        FetchMessages();
    }
 
    [RelayCommand]
    void OpenNewMessage()
    {
        Window newMessageWindow = new Window(new NewMessagePage());
        Application.Current.OpenWindow(newMessageWindow);
    }

    [RelayCommand]
    void FetchMessages()
    {
        string creds = FileSystemOperations.ReadTextFileSync("creds.txt");
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        List<EmailReceived> allEmails = ImapService.ReceiveEmailImap4(splitCreds[0], splitCreds[1]);
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

        MessageDisplayIsVisible = true;

        // Next line is needed to set the text on message display
        MarkAsText = ChooseMarkAsReadText(selectedEmail.MarkAsReadIcon);
        if (msg.MarkAsReadIcon) ToggleMarkAsRead();
    }

    [RelayCommand]
    void Reply()
    {
        Window newMessageWindow = new Window(new NewMessagePage(selectedEmail, false));
        Application.Current.OpenWindow(newMessageWindow);
    }

    [RelayCommand]
    void DeleteMessage() 
    {
        ImapService.DeleteMessage(messageFolder, selectedMessageId);
    }

    [RelayCommand]
    void ForwardMessage()
    {
        Window newMessageWindow = new Window(new NewMessagePage(selectedEmail, true));
        Application.Current.OpenWindow(newMessageWindow);
    }

    [RelayCommand]
    void ToggleMarkAsRead()
    {
        // Update UI
        selectedEmail.MarkAsReadIcon = !selectedEmail.MarkAsReadIcon;
        MarkAsText = ChooseMarkAsReadText(selectedEmail.MarkAsReadIcon);

        // Boolean switch to match naming, since the icon shows when email is unread
        ImapService.MarkOrUnmarkAsRead(
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
