using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarItemHistoryTests
{
    [Fact]
    public void Add_PutsNewestItemFirst()
    {
        var history = new AvatarItemHistory();

        history.Add("01040001");
        history.Add("01050002");

        Assert.Equal(new[] { "01050002", "01040001" }, history.Items);
    }

    [Fact]
    public void Add_MovesDuplicateToFrontWithoutDuplicatingIt()
    {
        var history = new AvatarItemHistory();
        history.Add("01040001");
        history.Add("01050002");

        history.Add("01040001");

        Assert.Equal(new[] { "01040001", "01050002" }, history.Items);
    }

    [Fact]
    public void Add_RemovesOldestItemWhenCapacityIsExceeded()
    {
        var history = new AvatarItemHistory(capacity: 3);
        history.Add("1");
        history.Add("2");
        history.Add("3");

        history.Add("4");

        Assert.Equal(new[] { "4", "3", "2" }, history.Items);
    }

    [Fact]
    public void Clear_RemovesEveryItem()
    {
        var history = new AvatarItemHistory();
        history.Add("01040001");

        history.Clear();

        Assert.Empty(history.Items);
    }

    [Fact]
    public void Constructor_RejectsNonPositiveCapacity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AvatarItemHistory(capacity: 0));
    }
}
