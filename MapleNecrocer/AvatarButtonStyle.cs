using System.Windows.Forms;

namespace MapleNecrocer;

internal static class AvatarButtonStyle
{
    internal static void Apply(Button button, ThemeMode mode)
    {
        button.BackColor = mode == ThemeMode.Dark
            ? System.Drawing.Color.FromArgb(0x8f, 0x96, 0x9e)
            : ThemePalette.For(mode).ControlBackground;
        button.UseVisualStyleBackColor = false;
    }
}
