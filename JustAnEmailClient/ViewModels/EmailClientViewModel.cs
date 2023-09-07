using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustAnEmailClient.Views;
using JustAnEmailClient.Services;
using JustAnEmailClient.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace JustAnEmailClient.ViewModels;

public partial class EmailClientViewModel : ObservableObject
{
    public ObservableCollection<EmailReceived> emailsReceived = new ObservableCollection<EmailReceived>();
    public ObservableCollection<EmailReceived> EmailsReceived
    {
        get => emailsReceived;
        set => SetProperty(ref emailsReceived, value);
    }

    [ObservableProperty]
    string selectedMessage = "";
    [ObservableProperty]
    string htmlBody = "";
 
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

        // List<EmailReceived> allEmails = MailReceiver.ReceiveEmailPop3(splitCreds[0], splitCreds[1]);
        List<EmailReceived> allEmails = MailReceiver.ReceiveEmailImap4(splitCreds[0], splitCreds[1]);
        EmailsReceived = new ObservableCollection<EmailReceived>(allEmails);
    }

    [RelayCommand]
    void SelectMessage(EmailReceived msg)
    {
        SelectedMessage = msg.BodyAsText;
        HtmlBody = msg.BodyAsHtml;
    }
}
