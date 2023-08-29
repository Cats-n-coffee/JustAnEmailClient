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
        var userInfo = new UserInfo();
        // userInfo.email = Email;
        // userInfo.password = Password;

        // mailSender.SendEmail(userInfo, SentTo, Subject, MessageContent);
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
