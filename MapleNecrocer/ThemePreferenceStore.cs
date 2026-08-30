namespace MapleNecrocer;

internal sealed class ThemePreferenceStore
{
    private readonly string filePath;

    internal ThemePreferenceStore(string filePath)
    {
        this.filePath = filePath;
    }

    internal static ThemePreferenceStore CreateDefault()
    {
        string directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MapleNecrocer");
        return new ThemePreferenceStore(Path.Combine(directory, "theme.txt"));
    }

    internal ThemeMode Load()
    {
        try
        {
            string value = File.ReadAllText(filePath).Trim();
            return Enum.TryParse(value, ignoreCase: true, out ThemeMode mode) &&
                   Enum.IsDefined(mode)
                ? mode
                : ThemeMode.Dark;
        }
        catch
        {
            return ThemeMode.Dark;
        }
    }

    internal bool Save(ThemeMode mode)
    {
        try
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, mode.ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }
}
