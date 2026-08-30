using System.Windows.Forms;

namespace MapleNecrocer;

internal static class AvatarSearchStyle
{
    internal static void ApplyPreviewSurface(Control control, ThemeMode mode)
    {
        control.BackColor = ThemePalette.For(mode).WindowBackground;
    }
}
