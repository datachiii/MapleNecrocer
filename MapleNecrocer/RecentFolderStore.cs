namespace MapleNecrocer;

internal sealed class RecentFolderStore
{
    private readonly string filePath;

    internal RecentFolderStore(string filePath)
    {
        this.filePath = filePath;
    }

    internal IReadOnlyList<string> Read()
    {
        if (!File.Exists(filePath))
        {
            return Array.Empty<string>();
        }

        try
        {
            return File.ReadLines(filePath)
                .Select(path => path.Trim())
                .Where(path => path.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
        catch (IOException)
        {
            return Array.Empty<string>();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<string>();
        }
    }

    internal string? FindMostRecentValid(Func<string, bool> isValid)
    {
        return Read().FirstOrDefault(isValid);
    }

    internal void Promote(string path)
    {
        string normalizedPath = path.Trim();
        if (normalizedPath.Length == 0)
        {
            return;
        }

        try
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            string[] paths = [normalizedPath, .. Read().Where(existing =>
                !string.Equals(existing, normalizedPath, StringComparison.OrdinalIgnoreCase))];
            File.WriteAllLines(filePath, paths);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}

internal static class MapleStoryFolderLocator
{
    internal static string? FindDataFile(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath) || !System.IO.Directory.Exists(folderPath))
        {
            return null;
        }

        try
        {
            string[] files = System.IO.Directory.GetFiles(folderPath, "*.wz", SearchOption.AllDirectories);
            return files.FirstOrDefault(path =>
                       string.Equals(Path.GetFileName(path), "Base.wz", StringComparison.OrdinalIgnoreCase))
                   ?? files.FirstOrDefault(path =>
                       string.Equals(Path.GetFileName(path), "Data.wz", StringComparison.OrdinalIgnoreCase));
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }
}
