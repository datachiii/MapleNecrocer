using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class StartupMapResolverTests
{
    [Fact]
    public void Resolve_WithMap9Directory_UsesFirstDigitDirectory()
    {
        string? inspectedPath = null;
        string? inspectedLinkPath = null;

        StartupMapResolution? result = StartupMapResolver.Resolve(
            "270000200",
            hasMap9Dir: true,
            path => { inspectedPath = path; return true; },
            path => { inspectedLinkPath = path; return null; });

        Assert.Equal("Map/Map/Map2/270000200.img", inspectedPath);
        Assert.Equal("Map/Map/Map2/270000200.img/info/link", inspectedLinkPath);
        Assert.Equal("270000200", result?.ResolvedMapId);
    }

    [Fact]
    public void Resolve_WithoutMap9Directory_UsesFlatMapDirectory()
    {
        StartupMapResolution? result = StartupMapResolver.Resolve(
            "270000200",
            hasMap9Dir: false,
            _ => true,
            _ => null);

        Assert.Equal("Map/Map/270000200.img", result?.NodePath);
    }

    [Fact]
    public void Resolve_WhenMapLinksToAnotherMap_ReturnsPaddedLinkedId()
    {
        StartupMapResolution? result = StartupMapResolver.Resolve(
            "270000200",
            hasMap9Dir: true,
            _ => true,
            _ => "123");

        Assert.Equal("000000123", result?.ResolvedMapId);
    }

    [Fact]
    public void Resolve_WhenMapNodeDoesNotExist_ReturnsNull()
    {
        StartupMapResolution? result = StartupMapResolver.Resolve(
            "270000200",
            hasMap9Dir: true,
            _ => false,
            _ => null);

        Assert.Null(result);
    }
}
