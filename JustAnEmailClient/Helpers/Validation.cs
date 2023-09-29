using System.Diagnostics;
using System.Text.RegularExpressions;

namespace JustAnEmailClient.Helpers;

public class Validation
{
    public static bool IsEmailValid(string email)
    {
        if (email.Length == 0) return false;
        // From SO
        string emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        return Regex.IsMatch(email, emailPattern);
    }
}
