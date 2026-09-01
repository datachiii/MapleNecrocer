using System.Drawing;
using System.Windows.Forms;

namespace MapleNecrocer;

internal static class GridHoverStyle
{
    internal static void ApplyEnter(DataGridViewRow row)
    {
        row.DefaultCellStyle.BackColor = ThemeManager.CurrentPalette.SelectionBackground;
    }

    internal static void ApplyLeave(DataGridViewRow row)
    {
        row.DefaultCellStyle.BackColor = Color.Empty;
    }
}
