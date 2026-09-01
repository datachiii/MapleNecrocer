using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class ThemePreferenceStoreTests : IDisposable
{
    private readonly string testDirectory = Path.Combine(
        Path.GetTempPath(),
        "MapleNecrocer.Tests",
        Guid.NewGuid().ToString("N"));

    [Fact]
    public void Load_MissingFile_DefaultsToDark()
    {
        var store = new ThemePreferenceStore(Path.Combine(testDirectory, "theme.txt"));

        Assert.Equal(ThemeMode.Dark, store.Load());
    }

    [Theory]
    [InlineData("Dark")]
    [InlineData("Light")]
    public void SaveAndLoad_RoundTripsMode(string value)
    {
        ThemeMode mode = Enum.Parse<ThemeMode>(value);
        var store = new ThemePreferenceStore(Path.Combine(testDirectory, "theme.txt"));

        Assert.True(store.Save(mode));
        Assert.Equal(mode, store.Load());
    }

    [Theory]
    [InlineData("")]
    [InlineData("Blue")]
    [InlineData("999")]
    public void Load_InvalidValue_DefaultsToDark(string value)
    {
        Directory.CreateDirectory(testDirectory);
        string path = Path.Combine(testDirectory, "theme.txt");
        File.WriteAllText(path, value);

        Assert.Equal(ThemeMode.Dark, new ThemePreferenceStore(path).Load());
    }

    [Fact]
    public void Save_WhenPathIsADirectory_ReturnsFalse()
    {
        Directory.CreateDirectory(testDirectory);
        var store = new ThemePreferenceStore(testDirectory);

        Assert.False(store.Save(ThemeMode.Light));
    }

    public void Dispose()
    {
        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }
}
