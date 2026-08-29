using System.Drawing;
using System.Windows.Forms;

namespace MapleNecrocer;

internal static class AvatarInventoryStyle
{
    internal static void Apply(DataGridView inventory)
    {
        inventory.BackgroundColor = AvatarItemPalette.CanvasBackground;
        inventory.DefaultCellStyle.BackColor = AvatarItemPalette.ItemBackground;
        inventory.RowsDefaultCellStyle.BackColor = AvatarItemPalette.ItemBackground;
        inventory.AlternatingRowsDefaultCellStyle.BackColor = AvatarItemPalette.ItemBackground;
        inventory.DefaultCellStyle.SelectionBackColor = AvatarItemPalette.ItemBackground;
        inventory.DefaultCellStyle.SelectionForeColor = Color.Black;
    }
}
