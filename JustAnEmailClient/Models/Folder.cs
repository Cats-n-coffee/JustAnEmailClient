using MailKit;

namespace JustAnEmailClient.Models;

public class Folder
{
    public string Id { get; set; }
    public string Name { get; set; }   
    public string ImageFile { get; set; }
    public IMailFolder MailFolder { get; set; }

    public static List<Folder> SortFolders(List<Folder> folders)
    {
        // Following list will break for non-outlook
        // Probably should make a model for each email provider
        List<string> order = new List<string>()
        { "inbox", "drafts", "sent", "junk", "deleted", "archive"};

        for (int i = 0; i < order.Count; i++)
        {
            for (int j = 0; j < folders.Count; j++)
            {
                // first find the key match by name
                // then check for the index -> might be wrong
                if (folders[j].Name.ToLower() == order[i] && j != i)
                {
                    var temp = folders[i]; // store the item at the index we need to replace
                    folders[i] = folders[j]; // place our current item (match) at the correct index i
                    folders[j] = temp; // wrong index receives non-matching item, to be sorted later
                }
            }
        }

        return folders;
    }
}
