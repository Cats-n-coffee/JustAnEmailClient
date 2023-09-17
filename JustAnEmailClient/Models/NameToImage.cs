using System;
using System.Collections.Generic;

namespace JustAnEmailClient.Models;
public class NameToImage
{
    public static readonly Dictionary<string, string> nameToImage =
        new Dictionary<string, string>()
        {
            {"archive", "archive.png"},
            {"deleted", "bin.png"},
            {"drafts", "draft.png"},
            {"inbox", "inbox.png"},
            {"junk", "junk.png"},
            {"notes", "note.png"},
            {"sent", "sent.png" },
        };
}
