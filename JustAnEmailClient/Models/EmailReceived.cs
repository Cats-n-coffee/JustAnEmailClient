namespace JustAnEmailClient.Models;

public class EmailReceived
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string DateSent { get; set; }
    public string messageId = "";
    public string BodyAsText { get; set; }
    public string BodyAsHtml { get; set; }
    public string BodyFirstChars { get; set; }
}
