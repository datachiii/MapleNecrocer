using System.Windows.Forms;

namespace MapleNecrocer;

internal static class AvatarInventoryStyle
{
    internal static void Apply(DataGridView inventory)
    {
        ThemeManager.SetRole(inventory, ThemeRole.AvatarInventory);
        ThemeManager.Apply(inventory);
    }
}
