using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarItemPaletteTests
{
    [Fact]
    public void For_Dark_UsesDeepCanvasAndReadableMediumTile()
    {
        ThemePalette palette = ThemePalette.For(ThemeMode.Dark);

        Assert.Equal(unchecked((int)0xFF272A2E), palette.AvatarCanvasBackground.ToArgb());
        Assert.Equal(unchecked((int)0xFF8F969E), palette.AvatarItemBackground.ToArgb());
    }

    [Fact]
    public void For_Light_RestoresExistingAvatarColors()
    {
        ThemePalette palette = ThemePalette.For(ThemeMode.Light);

        Assert.Equal(unchecked((int)0xFFC7C9CC), palette.AvatarCanvasBackground.ToArgb());
        Assert.Equal(unchecked((int)0xFFECECEC), palette.AvatarItemBackground.ToArgb());
    }

    [Fact]
    public void SemanticBorders_RemainYellowAndRed()
    {
        Assert.Equal(unchecked((int)0xFFFFD43B), AvatarItemPalette.EquippedBorder.ToArgb());
        Assert.Equal(unchecked((int)0xFFFF0000), AvatarItemPalette.SelectedBorder.ToArgb());
    }
}
