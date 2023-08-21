namespace JustAnEmailClient;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
        InitializeComponent();
		BindingContext = new LoginPageViewModel();
	}
}