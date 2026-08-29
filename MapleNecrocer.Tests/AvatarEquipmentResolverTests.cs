using MapleNecrocer;
using WzComparerR2.WzLib;
using Xunit;

namespace MapleNecrocer.Tests;

public class AvatarEquipmentResolverTests
{
    [Fact]
    public void Resolve_WhenEquipmentIsMissing_ReturnsIdPathAndUnavailableStatus()
    {
        string? requestedPath = null;
        var resolver = new AvatarEquipmentResolver(path =>
        {
            requestedPath = path;
            return null;
        });

        var result = resolver.Resolve("01005313");

        Assert.Equal("01005313", result.EquipmentId);
        Assert.Equal("Character/Cap/01005313.img", result.WzPath);
        Assert.Equal(result.WzPath, requestedPath);
        Assert.False(result.IsAvailable);
    }

    [Theory]
    [InlineData("00002012", "Character/00002012.img")]
    [InlineData("00012012", "Character/00012012.img")]
    [InlineData("01005313", "Character/Cap/01005313.img")]
    [InlineData("01703231", "Character/Weapon/01703231.img")]
    public void Resolve_BuildsTheSameCharacterPathAsCreateEquip(
        string equipmentId,
        string expectedPath)
    {
        string? requestedPath = null;
        var node = new Wz_Node(equipmentId + ".img");
        var resolver = new AvatarEquipmentResolver(path =>
        {
            requestedPath = path;
            return node;
        });

        var result = resolver.Resolve(equipmentId);

        Assert.Equal(expectedPath, result.WzPath);
        Assert.Equal(expectedPath, requestedPath);
        Assert.True(result.IsAvailable);
    }
}
