using JustAnEmailClient.ViewModels;
using JustAnEmailClient.Models;

namespace JustAnEmailClient.Views;

public partial class NewMessagePage : ContentPage
{
    public NewMessagePage()
    {
        InitializeComponent();
        BindingContext = new NewMessageViewModel();
    }
    public NewMessagePage(EmailReceived emailData, bool isForwarded, bool isReply)
	{
		InitializeComponent();
		BindingContext = new NewMessageViewModel(emailData, isForwarded, isReply);
	}
}