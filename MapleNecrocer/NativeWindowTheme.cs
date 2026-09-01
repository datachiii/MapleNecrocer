using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MapleNecrocer;

internal static class NativeWindowTheme
{
    private const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
    private const int DwmwaUseImmersiveDarkMode = 20;

    internal static void Apply(Form form, ThemeMode mode)
    {
        if (!form.IsHandleCreated)
        {
            return;
        }

        try
        {
            int enabled = mode == ThemeMode.Dark ? 1 : 0;
            int result = DwmSetWindowAttribute(
                form.Handle,
                DwmwaUseImmersiveDarkMode,
                ref enabled,
                sizeof(int));
            if (result != 0)
            {
                DwmSetWindowAttribute(
                    form.Handle,
                    DwmwaUseImmersiveDarkModeBefore20H1,
                    ref enabled,
                    sizeof(int));
            }
        }
        catch
        {
        }
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attribute,
        ref int attributeValue,
        int attributeSize);
}
