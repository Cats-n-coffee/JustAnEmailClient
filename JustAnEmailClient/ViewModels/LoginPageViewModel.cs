using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustAnEmailClient.Services;

namespace JustAnEmailClient;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string password = "";

    [RelayCommand]
    void Login()
    {
        /*
         This should be removed once the IMAP or POP3 server has been reached.
        Check how credentials are used, but logging in prior to using fetching anything
        shouldn't be necessary?
        For SMTP we can send the creds with the message sending?
        In the future on app start up, we will requests new messages from the server,
        creds can be send with that request?
        */
        if (Email != null && Password != null)
        {
            string combinedLine = $"{Email} - {Password}"; // Do some validation here
            FileSystemOperations.WriteToTextFile("creds.txt", combinedLine);
        }
    }
}
