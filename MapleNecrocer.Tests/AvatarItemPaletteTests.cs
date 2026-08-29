using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarItemPaletteTests
{
    [Fact]
    public void ApprovedColors_HaveExpectedArgb()
    {
        Assert.Equal(unchecked((int)0xFFC7C9CC), AvatarItemPalette.CanvasBackground.ToArgb());
        Assert.Equal(unchecked((int)0xFFECECEC), AvatarItemPalette.ItemBackground.ToArgb());
        Assert.Equal(unchecked((int)0xFFFFD43B), AvatarItemPalette.EquippedBorder.ToArgb());
        Assert.Equal(unchecked((int)0xFF5F4B00), AvatarItemPalette.EquippedOutline.ToArgb());
        Assert.Equal(unchecked((int)0xFFFF0000), AvatarItemPalette.SelectedBorder.ToArgb());
    }
}
