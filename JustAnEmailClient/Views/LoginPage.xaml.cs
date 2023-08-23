using JustAnEmailClient.ViewModels;

namespace JustAnEmailClient.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
        InitializeComponent();
		BindingContext = new LoginPageViewModel();
	}
}