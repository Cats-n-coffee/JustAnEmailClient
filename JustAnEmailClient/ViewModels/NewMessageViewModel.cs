using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using JustAnEmailClient.Services;
using JustAnEmailClient.Models;

namespace JustAnEmailClient.ViewModels;

public partial class NewMessageViewModel : ObservableObject
{
    [ObservableProperty]
    string sentTo = "";
    [ObservableProperty]
    string subject = "";
    [ObservableProperty]
    string messageContent = "";

    MailSender mailSender = new MailSender();

    [RelayCommand]
    void Send()
    {
        // Temporary
        string creds = FileSystemOperations.ReadTextFileSync("creds.txt");
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        var userInfo = new UserInfo();
        userInfo.email = splitCreds[0];
        userInfo.password = splitCreds[1];

        mailSender.SendEmail(userInfo, SentTo, Subject, MessageContent);
        // TODO: close the current window after email is sent
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
