using JustAnEmailClient.Services;
using JustAnEmailClient.Views;

namespace JustAnEmailClient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new AppShell());
    }
    /*
    protected override void OnLaunched()
    {
        if (CheckCredentials())
        {
            Current.MainPage.Navigation.PushAsync(new EmailClientPage());
        }
        else
        {
            Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }

    private static bool CheckCredentials()
    {
        // Implement your logic to check the text file for credentials.
        // Return true if credentials are present, otherwise false.
        // You can read the file and parse its contents to determine credentials.
        // Example:
        // if (File.Exists("credentials.txt"))
        // {
        //     var credentials = File.ReadAllLines("credentials.txt");
        //     return credentials.Length > 0; // Check if there are any credentials.
        // }
        // return false;

        // string textFileContents = FileSystemOperations.ReadTextFileSync("creds.txt");
        // Debug.WriteLine($"textfile: {textFileContents}");
        string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "creds.txt");
        // Debug.WriteLine($"filePath: {filePath}");
        if (File.Exists(filePath))
        {
            string textFileContents = FileSystemOperations.ReadTextFileSync("creds.txt");
            return textFileContents.Length > 0;
        }

        return false;
    }*/
}