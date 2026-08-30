using Manina.Windows.Forms;
using System.Drawing;

namespace MapleNecrocer;

internal static class AvatarItemBrowserStyle
{
    internal static void Apply(ImageListView list, ThemeMode mode)
    {
        Color canvas = AvatarItemPalette.CanvasBackgroundFor(mode);
        Color item = AvatarItemPalette.ItemBackgroundFor(mode);
        list.BackColor = canvas;
        list.Colors.ControlBackColor = canvas;
        list.Colors.BackColor = item;
        list.Colors.AlternateBackColor = item;
        list.Colors.HoverColor1 = item;
        list.Colors.HoverColor2 = item;
        list.Colors.SelectedBorderColor = AvatarItemPalette.SelectedBorder;
        list.Invalidate();
    }
}
