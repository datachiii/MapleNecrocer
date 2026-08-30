using System.Drawing;

namespace MapleNecrocer;

internal enum ThemeMode
{
    Dark,
    Light,
}

internal sealed record ThemePalette(
    Color WindowBackground,
    Color ControlBackground,
    Color InputBackground,
    Color Foreground,
    Color Border,
    Color SelectionBackground,
    Color SelectionForeground,
    Color Grid,
    Color AvatarCanvasBackground,
    Color AvatarItemBackground)
{
    internal static ThemePalette For(ThemeMode mode) => mode switch
    {
        ThemeMode.Light => new ThemePalette(
            Color.FromArgb(0xF0, 0xF0, 0xF0),
            Color.White,
            Color.White,
            Color.FromArgb(0x20, 0x21, 0x24),
            Color.FromArgb(0xAE, 0xB4, 0xBA),
            Color.FromArgb(0x00, 0x78, 0xD7),
            Color.White,
            Color.FromArgb(0xD0, 0xD4, 0xD8),
            Color.FromArgb(0xC7, 0xC9, 0xCC),
            Color.FromArgb(0xEC, 0xEC, 0xEC)),
        _ => new ThemePalette(
            Color.FromArgb(0x20, 0x22, 0x25),
            Color.FromArgb(0x2F, 0x33, 0x38),
            Color.FromArgb(0x3A, 0x3F, 0x45),
            Color.FromArgb(0xF1, 0xF3, 0xF5),
            Color.FromArgb(0x55, 0x5B, 0x62),
            Color.FromArgb(0x3D, 0x6F, 0xA8),
            Color.White,
            Color.FromArgb(0x55, 0x5B, 0x62),
            Color.FromArgb(0x27, 0x2A, 0x2E),
            Color.FromArgb(0x8F, 0x96, 0x9E)),
    };
}
