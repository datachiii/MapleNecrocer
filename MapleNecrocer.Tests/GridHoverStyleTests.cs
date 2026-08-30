using MapleNecrocer;
using System.Drawing;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class GridHoverStyleTests
{
    [Theory]
    [InlineData("Dark")]
    [InlineData("Light")]
    public void ApplyLeave_ClearsHoverOverrideAndRestoresInheritedTheme(string modeName)
    {
        ThemeMode mode = Enum.Parse<ThemeMode>(modeName);
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(mode);
            using var grid = new DataGridView();
            grid.Columns.Add(new DataGridViewTextBoxColumn());
            grid.Rows.Add("Map");
            ThemeManager.Apply(grid);
            DataGridViewRow row = grid.Rows[0];

            GridHoverStyle.ApplyEnter(row);
            Assert.Equal(
                ThemePalette.For(mode).SelectionBackground,
                row.DefaultCellStyle.BackColor);

            GridHoverStyle.ApplyLeave(row);

            Assert.Equal(Color.Empty, row.DefaultCellStyle.BackColor);
            Assert.Equal(
                ThemePalette.For(mode).InputBackground,
                row.InheritedStyle.BackColor);
        });
    }
}
