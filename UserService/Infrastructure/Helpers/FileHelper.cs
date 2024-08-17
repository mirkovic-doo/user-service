namespace UserService.Infrastructure.Helpers;

public static class FileHelper
{
    public static List<string> GetFileNamesFromFolder(string folderName)
    {
        var fileNames = Directory.GetFiles(folderName);

        var fileNamesWithoutPathAndExtension = fileNames
            .Select(Path.GetFileNameWithoutExtension);

        var orderedFileNames = fileNamesWithoutPathAndExtension.OrderBy(fileName => fileName);

        return orderedFileNames.OrderBy(fileName => fileName).ToList();
    }
}
