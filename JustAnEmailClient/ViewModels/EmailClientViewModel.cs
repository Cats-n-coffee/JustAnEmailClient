using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnEmailClient.ViewModels;

public partial class EmailClientViewModel : ObservableObject
{
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string password = "";
    [ObservableProperty]
    string messageContent = "";

    [RelayCommand]
    void Send()
    {
        Debug.WriteLine("Send clicked");
    }
}
