using System.IO;

namespace PlanA.Web.Core.Helper;

public class FileHelper
{
    public static void DeleteFolder(string path)
    {
        if (Directory.Exists(path))
        {
            DeleteFilesFromDirectory(path);
        }
    }
    
    public static void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    
    public static string CreateFolder(string rootFolderName, string featureName)
    {
        var rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), rootFolderName);
        var directory = Path.Combine(rootDirectory, featureName);

        CreateFolder(rootDirectory);
        CreateFolder(directory);
        return directory;
    }

    private static void DeleteFilesFromDirectory(string folderPath)
    {
        var directoryInfo = new DirectoryInfo(folderPath);

        foreach (var file in directoryInfo.GetFiles())
        {
            file.Delete();
        }

        foreach (var dir in directoryInfo.GetDirectories())
        {
            dir.Delete(true);
        }
    }
}