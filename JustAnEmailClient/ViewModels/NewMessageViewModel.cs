using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using JustAnEmailClient.Services;
using JustAnEmailClient.Models;

namespace JustAnEmailClient.ViewModels;

public partial class NewMessageViewModel : ObservableObject
{
    static string[] splitCreds = RetrieveCredsFromFile("creds.txt");

    [ObservableProperty]
    string senderEmail = splitCreds[0];
    [ObservableProperty]
    string sentTo = "";
    [ObservableProperty]
    string subject = "";
    [ObservableProperty]
    string messageContent = "";

    MailSender mailSender = new MailSender();

    public NewMessageViewModel()
    {
        SentTo = "";
    }
    public NewMessageViewModel(string sentTo)
    { 
        SentTo = sentTo;
    }

    [RelayCommand]
    void Send()
    {
        // Temporary until we have some state management
        var userInfo = new UserInfo();
        userInfo.email = splitCreds[0];
        userInfo.password = splitCreds[1];

        mailSender.SendEmail(userInfo, SentTo, Subject, MessageContent);
        // TODO: close the current window after email is sent
    }

    static string[] RetrieveCredsFromFile(string filename)
    {
        string creds = FileSystemOperations.ReadTextFileSync(filename);
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        return splitCreds;
    }

    /*
    [RelayCommand]
    void Which()
    {
        Debug.WriteLine(Application.Current.Windows);
        Debug.WriteLine(Application.Current.Windows.Count);
        Debug.WriteLine(Microsoft.Maui.Controls.Window.PageProperty);
    }
    */
}
