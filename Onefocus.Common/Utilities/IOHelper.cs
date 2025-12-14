using System.Reflection;

namespace Onefocus.Common.Utilities;

public class IOHelper
{

    public static string GetApplicationFolder()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new NullReferenceException("Application Folder is null.");
    }

    public static string GetFilePath(params string[] paths)
    {
        var rootFolder = GetApplicationFolder();
        if (paths == null || paths.Length == 0) return rootFolder;

        if (paths[0] == rootFolder) rootFolder = string.Empty;

        return Path.Combine([rootFolder, .. paths]);
    }

    public static bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }
}
