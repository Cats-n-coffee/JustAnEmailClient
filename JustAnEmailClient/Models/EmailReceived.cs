using MailKit;

namespace JustAnEmailClient.Models;

public class EmailReceived
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string DateSent { get; set; }
    public string MessageId {  get; set; }
    public string BodyAsText { get; set; }
    public string BodyAsHtml { get; set; }
    public bool WasRead { get; set; }

    public IMailFolder MessageFolder { get; set; }
}
