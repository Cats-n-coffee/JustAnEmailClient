using CommunityToolkit.Mvvm.ComponentModel;
using JustAnEmailClient.Services;
using JustAnEmailClient.Views;
using System.Diagnostics;

namespace JustAnEmailClient.ViewModels;

public partial class LoadingPageViewModel : ObservableObject
{
    // Ideally, this is the page that would check for creds
    // to be stored and decide which page to navigate to
    public LoadingPageViewModel() {
        Reroute();
    }
    public async void Reroute()
    {
        string credentials = FileSystemOperations.ReadTextFileSync("creds.txt");
        Debug.WriteLine(credentials);
        Debug.WriteLine(DeviceInfo.Platform);

        if (credentials != null && credentials.Length > 1)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Shell.Current.Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync($"//{nameof(EmailClientPage)}");
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
