using System.Drawing;

namespace MapleNecrocer;

internal static class AvatarItemPalette
{
    internal static Color CanvasBackground => Color.FromArgb(0xC7, 0xC9, 0xCC);
    internal static Color ItemBackground => Color.FromArgb(0xEC, 0xEC, 0xEC);
    internal static Color EquippedBorder => Color.FromArgb(0xFF, 0xD4, 0x3B);
    internal static Color EquippedOutline => Color.FromArgb(0x5F, 0x4B, 0x00);
    internal static Color SelectedBorder => Color.Red;
}
