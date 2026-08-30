using Manina.Windows.Forms;
using MapleNecrocer;
using System.Drawing;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class ThemePaletteTests
{
    [Fact]
    public void For_ReturnsDistinctDarkAndLightSurfaces()
    {
        ThemePalette dark = ThemePalette.For(ThemeMode.Dark);
        ThemePalette light = ThemePalette.For(ThemeMode.Light);

        Assert.NotEqual(dark.WindowBackground, light.WindowBackground);
        Assert.NotEqual(dark.ControlBackground, light.ControlBackground);
        Assert.NotEqual(dark.Foreground, light.Foreground);
    }

    [Fact]
    public void AvatarItemBrowserStyle_AppliesModeWithoutChangingImages()
    {
        StaTest.Run(() =>
        {
            AppContext.SetSwitch(
                "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization",
                true);
            using var image = new Bitmap(2, 2);
            using var list = new ImageListView();
            image.SetPixel(0, 0, Color.Magenta);
            list.Items.Add("item", image);

            AvatarItemBrowserStyle.Apply(list, ThemeMode.Dark);

            ThemePalette palette = ThemePalette.For(ThemeMode.Dark);
            Assert.Equal(palette.AvatarCanvasBackground, list.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, list.Colors.ControlBackColor);
            Assert.Equal(palette.AvatarItemBackground, list.Colors.BackColor);
            Assert.Equal(palette.AvatarItemBackground, list.Colors.AlternateBackColor);
            Assert.Equal(Color.Magenta.ToArgb(), image.GetPixel(0, 0).ToArgb());
        });
    }
}
