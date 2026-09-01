using System.Drawing;

namespace MapleNecrocer;

internal static class AvatarItemPalette
{
    internal static Color CanvasBackgroundFor(ThemeMode mode) =>
        ThemePalette.For(mode).AvatarCanvasBackground;

    internal static Color ItemBackgroundFor(ThemeMode mode) =>
        ThemePalette.For(mode).AvatarItemBackground;

    internal static Color EquippedBorder => Color.FromArgb(0xFF, 0xD4, 0x3B);
    internal static Color EquippedOutline => Color.FromArgb(0x5F, 0x4B, 0x00);
    internal static Color SelectedBorder => Color.Red;
}
