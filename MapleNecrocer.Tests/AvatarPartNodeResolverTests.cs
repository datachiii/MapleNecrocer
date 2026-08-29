using MapleNecrocer;
using WzComparerR2.WzLib;
using Xunit;

namespace MapleNecrocer.Tests;

public class AvatarPartNodeResolverTests
{
    [Fact]
    public void GetChildren_WhenPathCannotBeResolved_ReturnsEmptyAndReportsPath()
    {
        string? reportedPath = null;
        var resolver = new AvatarPartNodeResolver(
            _ => null,
            path => reportedPath = path);

        var children = resolver.GetChildren("Character/Cap/01000000.img");

        Assert.Empty(children);
        Assert.Equal("Character/Cap/01000000.img", reportedPath);
    }

    [Fact]
    public void GetChildren_WhenPathResolves_ReturnsTheNodeChildrenWithoutReporting()
    {
        var parent = new Wz_Node("01000000.img");
        var child = new Wz_Node("info");
        parent.Nodes.Add(child);
        var reportedPaths = new List<string>();
        var resolver = new AvatarPartNodeResolver(
            _ => parent,
            reportedPaths.Add);

        var children = resolver.GetChildren("Character/Cap/01000000.img");

        Assert.Same(child, Assert.Single(children));
        Assert.Empty(reportedPaths);
    }
}
