using JustAnEmailClient.ViewModels;

namespace JustAnEmailClient.Views;

public partial class NewMessagePage : ContentPage
{
	public NewMessagePage()
	{
		InitializeComponent();
		BindingContext = new NewMessageViewModel();
	}
}