using System.Windows.Forms;

namespace MapleNecrocer;

internal static class AvatarGenderInputPolicy
{
    internal static bool ShouldSuppressComboBoxNavigation(Keys key) =>
        key is Keys.Left or Keys.Right;
}
