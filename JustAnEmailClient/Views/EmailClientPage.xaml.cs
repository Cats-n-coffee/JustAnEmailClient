using JustAnEmailClient.ViewModels;

namespace JustAnEmailClient.Views;

public partial class EmailClientPage : ContentPage
{
	public EmailClientPage()
	{
		InitializeComponent();
		BindingContext = new EmailClientViewModel();
	}
}