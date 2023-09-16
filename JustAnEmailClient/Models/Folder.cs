using MailKit;

namespace JustAnEmailClient.Models;

public class Folder
{
    public string Id { get; set; }
    public string Name { get; set; }   
    public string ImageFile { get; set; }
    public IMailFolder MailFolder { get; set; }
}
