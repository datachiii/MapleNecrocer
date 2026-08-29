using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class RecentFolderStoreTests : IDisposable
{
    private readonly string testDirectory = Path.Combine(
        Path.GetTempPath(),
        "MapleNecrocer.Tests",
        Guid.NewGuid().ToString("N"));

    [Fact]
    public void Read_TrimsBlankLinesAndCaseInsensitiveDuplicates()
    {
        Directory.CreateDirectory(testDirectory);
        string filePath = Path.Combine(testDirectory, "RecentFiles.txt");
        File.WriteAllLines(filePath, ["  C:\\Maple  ", "", "c:\\maple", "D:\\Game"]);

        var store = new RecentFolderStore(filePath);

        Assert.Equal(["C:\\Maple", "D:\\Game"], store.Read());
    }

    [Fact]
    public void FindMostRecentValid_ReturnsFirstEntryAcceptedByValidator()
    {
        Directory.CreateDirectory(testDirectory);
        string filePath = Path.Combine(testDirectory, "RecentFiles.txt");
        File.WriteAllLines(filePath, ["C:\\Missing", "D:\\Maple", "E:\\Older"]);
        var store = new RecentFolderStore(filePath);

        string? result = store.FindMostRecentValid(path => path == "D:\\Maple");

        Assert.Equal("D:\\Maple", result);
    }

    [Fact]
    public void Promote_MovesFolderToFirstAndRemovesDuplicates()
    {
        Directory.CreateDirectory(testDirectory);
        string filePath = Path.Combine(testDirectory, "RecentFiles.txt");
        File.WriteAllLines(filePath, ["C:\\First", "D:\\Maple", "E:\\Other"]);
        var store = new RecentFolderStore(filePath);

        store.Promote("d:\\maple");

        Assert.Equal(["d:\\maple", "C:\\First", "E:\\Other"], File.ReadAllLines(filePath));
    }

    [Fact]
    public void FindDataFile_PrefersBaseWzAndFallsBackToDataWz()
    {
        string baseFolder = Path.Combine(testDirectory, "base");
        string dataFolder = Path.Combine(testDirectory, "data");
        Directory.CreateDirectory(baseFolder);
        Directory.CreateDirectory(dataFolder);
        string baseFile = Path.Combine(baseFolder, "Base.wz");
        string dataFile = Path.Combine(dataFolder, "Data.wz");
        File.WriteAllText(baseFile, "");
        File.WriteAllText(dataFile, "");

        Assert.Equal(baseFile, MapleStoryFolderLocator.FindDataFile(baseFolder));
        Assert.Equal(dataFile, MapleStoryFolderLocator.FindDataFile(dataFolder));
    }

    public void Dispose()
    {
        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }
}
