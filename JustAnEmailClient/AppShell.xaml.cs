using JustAnEmailClient.Services;
using JustAnEmailClient.Views;
using System.Diagnostics;

namespace JustAnEmailClient;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EmailClientPage), typeof(EmailClientPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

        // FindStartUpRoute();
    }

    public async static void FindStartUpRoute()
    {
        // This will be removed once POP3 or IMAP are setup
        string textFileContents = FileSystemOperations.ReadTextFileSync("creds.txt");
        Debug.WriteLine($"textfile: {textFileContents}");

        // if textFileContents -> navigates to email view
        if ( textFileContents != null )
        {
            // await Current.GoToAsync($"//{nameof(EmailClientPage)}");
            await Shell.Current.Navigation.PushAsync(new EmailClientPage());
        } else
        {
            // await Current.GoToAsync($"//{nameof(LoginPage)}");
            await Shell.Current.Navigation.PushAsync(new LoginPage());
        }
        // else login page
    }
}