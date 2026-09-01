using System.Windows.Forms;

namespace MapleNecrocer;

public class ThemedForm : Form
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ThemeManager.Apply(this);
    }
}
