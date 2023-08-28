using JustAnEmailClient.ViewModels;

namespace JustAnEmailClient.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
		BindingContext = new LoadingPageViewModel();
	}
}