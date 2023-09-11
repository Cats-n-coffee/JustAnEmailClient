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

    // Visibily of UI elements
    [ObservableProperty]
    bool isEditorVisible = true;
    [ObservableProperty]
    bool isWebViewVisible = false;
    [ObservableProperty]
    bool isTextViewVisible = false;
    [ObservableProperty]
    int editorMinHeight = 350;

    // Content of existing messages
    [ObservableProperty]
    string textBody = "";
    [ObservableProperty]
    string htmlBody = "";

    MailSender mailSender = new MailSender();

    public NewMessageViewModel() {}
    public NewMessageViewModel(EmailReceived emailData, bool isForwarded)
    {
        if (isForwarded) SentTo = "";
        else SentTo = emailData?.Sender;

        if (isForwarded) Subject = $"Fwd: {emailData?.Subject}";
        else Subject = $"Re: {emailData?.Subject}";

        if (emailData?.BodyAsHtml != null)
        {
            HtmlBody = emailData?.BodyAsHtml;
            IsWebViewVisible = true;
            IsTextViewVisible = false;
        } else
        {
            TextBody = emailData?.BodyAsText;
            IsWebViewVisible = false;
            IsTextViewVisible = true;
        }

        EditorMinHeight = 100;
    }

    [RelayCommand]
    void Send()
    {
        // Temporary until we have some state management
        var userInfo = new UserInfo();
        userInfo.email = splitCreds[0];
        userInfo.password = splitCreds[1];

        mailSender.SendEmail(userInfo, SentTo, Subject, MessageContent);
        // TODO: try again in the future, GetParentWindow not found
        Application.Current?.CloseWindow(Application.Current.Windows[1]);
        // Application.Current.CloseWindow(GetParentWindow());
    }

    static string[] RetrieveCredsFromFile(string filename)
    {
        string creds = FileSystemOperations.ReadTextFileSync(filename);
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        return splitCreds;
    }
}
