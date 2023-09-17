using CommunityToolkit.Mvvm.ComponentModel;
using MailKit;
using MimeKit;

namespace JustAnEmailClient.Models;

public partial class EmailReceived : ObservableObject
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string DateSent { get; set; }
    public string MessageId {  get; set; }
    public string BodyAsText { get; set; }
    public string BodyAsHtml { get; set; }
    public MimeMessage OriginalMessage { get; set; }
    
    private bool _markAsReadIcon = false;
    public bool MarkAsReadIcon
    {
        get => _markAsReadIcon;
        set => SetProperty(ref _markAsReadIcon, value);
    }

    public IMailFolder MessageFolder { get; set; }
}
