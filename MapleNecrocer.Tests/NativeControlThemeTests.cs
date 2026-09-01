using MapleNecrocer;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class NativeControlThemeTests
{
    [Fact]
    public void GetThemeClass_DarkModeUsesControlSpecificDarkClasses()
    {
        StaTest.Run(() =>
        {
            using var comboBox = new ComboBox();
            using var verticalScrollBar = new VScrollBar();

            Assert.Equal(
                "DarkMode_CFD",
                NativeControlTheme.GetThemeClass(comboBox, ThemeMode.Dark));
            Assert.Equal(
                "DarkMode_Explorer",
                NativeControlTheme.GetThemeClass(verticalScrollBar, ThemeMode.Dark));
        });
    }

    [Fact]
    public void GetThemeClass_LightModeRemovesNativeOverride()
    {
        StaTest.Run(() =>
        {
            using var verticalScrollBar = new VScrollBar();

            Assert.Null(
                NativeControlTheme.GetThemeClass(verticalScrollBar, ThemeMode.Light));
        });
    }
}
