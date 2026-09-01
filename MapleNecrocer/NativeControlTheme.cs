using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MapleNecrocer;

internal static class NativeControlTheme
{
    internal static string? GetThemeClass(Control control, ThemeMode mode)
    {
        if (mode == ThemeMode.Light)
        {
            return null;
        }

        return control is ComboBox
            ? "DarkMode_CFD"
            : "DarkMode_Explorer";
    }

    internal static void Apply(Control control, ThemeMode mode)
    {
        if (!control.IsHandleCreated || !IsSupported(control))
        {
            return;
        }

        try
        {
            SetWindowTheme(control.Handle, GetThemeClass(control, mode), null);
        }
        catch
        {
        }
    }

    private static bool IsSupported(Control control) => control is
        ComboBox or
        ScrollBar or
        TextBoxBase or
        ListBox or
        ListView or
        TreeView or
        DataGridView;

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(
        IntPtr windowHandle,
        string? subAppName,
        string? subIdList);
}
