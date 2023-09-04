namespace JustAnEmailClient.Models;

public class EmailReceived
{
    public string Subject { get; set; }
    public string DateSent { get; set; }
    public string messageId = "";
    public string bodyAsText = "";
}
