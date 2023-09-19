using MimeKit;

namespace JustAnEmailClient.Models;

public class MessageToSend
{
    public string Sender { get; set; } = string.Empty;
    public string Recipient {  get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;

    public string BodyAsHtml { get; set; } = string.Empty;
    public MimeMessage OriginalMessage { get; set; }
}
