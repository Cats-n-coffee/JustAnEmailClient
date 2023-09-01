using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustAnEmailClient.Views;
using System.Diagnostics;

namespace JustAnEmailClient.ViewModels;

public partial class EmailClientViewModel : ObservableObject
{
    [RelayCommand]
    void OpenNewMessage()
    {
        Debug.WriteLine(Application.Current.Windows.Count);
        Window newMessageWindow = new Window(new NewMessagePage());
        Application.Current.OpenWindow(newMessageWindow);
    }
}
