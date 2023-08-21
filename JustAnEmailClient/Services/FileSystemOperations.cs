using System.Diagnostics;
namespace JustAnEmailClient.Services;

public class FileSystemOperations
{
    public static async void WriteToTextFile(string fileName, string text) {
        string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

        bool doesFileExist = File.Exists(filePath);
        if (doesFileExist)
        {
            File.Delete(filePath);
        }
        
        using FileStream outputStream = File.OpenWrite(filePath);
        using StreamWriter streamWriter = new StreamWriter(outputStream);
        
        await streamWriter.WriteAsync(text);
    }

    public static string ReadTextFileSync(string fileName)
    {
        string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

        // using FileStream fileStream = File.OpenRead(filePath);
        var reader = new StreamReader(filePath);
        var fileContents = reader.ReadToEnd();
        reader.Close();

        return fileContents;
    }
}
