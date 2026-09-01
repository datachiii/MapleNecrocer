using System.Drawing;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class AvatarSearchStyleTests
{
    [Theory]
    [InlineData("Dark")]
    [InlineData("Light")]
    public void ApplyPreviewSurface_UsesThemeWindowBackground(string modeName)
    {
        StaTest.Run(() =>
        {
            ThemeMode mode = Enum.Parse<ThemeMode>(modeName);
            using var control = new Panel();
            AvatarSearchStyle.ApplyPreviewSurface(control, mode);
            Assert.Equal(ThemePalette.For(mode).WindowBackground, control.BackColor);
        });
    }
}
