using CommunityToolkit.Mvvm.ComponentModel;
using JustAnEmailClient.Services;
using JustAnEmailClient.Views;

namespace JustAnEmailClient.ViewModels;

public partial class LoadingPageViewModel : ObservableObject
{
    string credentials = string.Empty;
    public LoadingPageViewModel() {
        credentials = FileSystemOperations.ReadTextFileSync("creds.txt");

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
