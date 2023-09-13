using CommunityToolkit.Mvvm.ComponentModel;
using JustAnEmailClient.Services;
using JustAnEmailClient.Views;
using System.Diagnostics;

namespace JustAnEmailClient.ViewModels;

public partial class LoadingPageViewModel : ObservableObject
{
    ImapService imapServiceInstance = null;
    string credentials = string.Empty;
    public LoadingPageViewModel() {
        credentials = FileSystemOperations.ReadTextFileSync("creds.txt");
        string[] splitCreds = FileSystemOperations.SeparateEmailAndPassword(credentials);

        imapServiceInstance = new ImapService(splitCreds[0], splitCreds[1]);

        Reroute();
    }
    public async void Reroute()
    {
        

        if (credentials != null && credentials.Length > 1)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Shell.Current.Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync(
                        $"//{nameof(EmailClientPage)}",
                        true,
                        new Dictionary<string, object> { { "ImapServiceInstance", imapServiceInstance } }
                    );
                });
            } else
            {
                await Shell.Current.GoToAsync($"//{nameof(EmailClientPage)}");
            }
        } else
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Shell.Current.Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                });
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
