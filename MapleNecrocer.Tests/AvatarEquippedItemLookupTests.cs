using MapleNecrocer;
using Manina.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarEquippedItemLookupTests
{
    [Fact]
    public void AvatarItemRenderer_UsesNativeImageListViewRenderer()
    {
        Assert.Equal(
            typeof(ImageListView.ImageListViewRenderer),
            typeof(AvatarItemRenderer).BaseType);
    }

    [Fact]
    public void IsEquipped_ReturnsTrueOnlyForCurrentEquipment()
    {
        var lookup = new AvatarEquippedItemLookup();
        lookup.ReplaceWith(new[] { "00002000", "01052610" });

        Assert.True(lookup.IsEquipped("01052610"));
        Assert.False(lookup.IsEquipped("01072751"));
        Assert.False(lookup.IsEquipped(null));
    }

    [Fact]
    public void ReplaceWith_RemovesEquipmentThatIsNoLongerPresent()
    {
        var lookup = new AvatarEquippedItemLookup();
        lookup.ReplaceWith(new[] { "01052610" });

        lookup.ReplaceWith(new[] { "01072751" });

        Assert.False(lookup.IsEquipped("01052610"));
        Assert.True(lookup.IsEquipped("01072751"));
    }

    [Theory]
    [InlineData(false, false, 0)]
    [InlineData(true, false, 1)]
    [InlineData(false, true, 2)]
    [InlineData(true, true, 3)]
    public void ResolveBorderState_KeepsEquippedAndSelectedIndependent(
        bool equipped,
        bool selected,
        int expected)
    {
        var lookup = new AvatarEquippedItemLookup();
        lookup.ReplaceWith(equipped ? new[] { "01052610" } : Array.Empty<string>());

        Assert.Equal(
            (AvatarItemBorderState)expected,
            AvatarItemBorderPolicy.Resolve(lookup, "01052610", selected));
    }
}
