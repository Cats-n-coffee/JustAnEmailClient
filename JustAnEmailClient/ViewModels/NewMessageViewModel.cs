using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using JustAnEmailClient.Services;
using JustAnEmailClient.Models;
using JustAnEmailClient.Helpers;

namespace JustAnEmailClient.ViewModels;

public partial class NewMessageViewModel : ObservableObject
{
    static string[] splitCreds = RetrieveCredsFromFile("creds.txt");

    EmailReceived emailToReplyOrForward = null;
    bool isForwardedMsg = false;
    bool isReplyMsg = false;

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
    [ObservableProperty]
    string sentToErrorBackground = "White";

    // Content of existing messages
    [ObservableProperty]
    string textBody = "";
    [ObservableProperty]
    string htmlBody = "";

    public NewMessageViewModel() {}
    public NewMessageViewModel(EmailReceived emailData, bool isForwarded, bool isReply)
    {
        emailToReplyOrForward = emailData;

        if (isReply) isReplyMsg = true;

        if (isForwarded)
        {
            SentTo = "";
            isForwardedMsg = true;
        }
        else SentTo = emailData?.Sender;

        if (isForwarded) Subject = $"Fwd: {emailData?.Subject}";
        else Subject = $"Re: {emailData?.Subject}";

        if (emailData?.BodyAsHtml != null)
        {
            var visitor = new HtmlPreviewVisitor();
            emailData?.OriginalMessage.Accept(visitor);
            HtmlBody = visitor.HtmlBody;
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
        bool isValid = Validation.IsEmailValid(SentTo);
        Debug.WriteLine($"inside send is valid {isValid}");
        if (!isValid)
        {
            SentToErrorBackground = "Red";

            TimeoutHelper.SetTimeout(() =>
            {
                SentToErrorBackground = "White";
            }, 2000);
        } else
        {
            // Temporary until we have some state management
            var userInfo = new UserInfo();
            userInfo.email = splitCreds[0];
            userInfo.password = splitCreds[1];

            var newMessage = new MessageToSend();
            newMessage.Sender = userInfo.email;
            newMessage.Subject = Subject;
            newMessage.Recipient = SentTo;
            newMessage.MessageContent = MessageContent; // This is the content we just typed

            if (emailToReplyOrForward != null)
            {
                newMessage.OriginalMessage = emailToReplyOrForward.OriginalMessage; // This is some original message
            }

            MailSender.SendEmail(userInfo, newMessage, isForwardedMsg, isReplyMsg);
            // TODO: try again in the future, GetParentWindow not found
            Application.Current?.CloseWindow(Application.Current.Windows[1]);
            // Application.Current.CloseWindow(GetParentWindow());
        }
    }

    static string[] RetrieveCredsFromFile(string filename)
    {
        string creds = FileSystemOperations.ReadTextFileSync(filename);
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(creds);

        return splitCreds;
    }
}
