using MapleNecrocer;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

[Collection(ThemeTestCollection.Name)]
public sealed class AvatarInventoryStyleTests
{
    [Theory]
    [InlineData("Dark")]
    [InlineData("Light")]
    public void Apply_UsesActiveAvatarPaletteAndSurvivesRetheme(string modeName)
    {
        ThemeMode mode = Enum.Parse<ThemeMode>(modeName);
        StaTest.Run(() =>
        {
            ThemeManager.Initialize(mode);
            using var inventory = new DataGridView();
            inventory.Columns.Add(new DataGridViewTextBoxColumn());
            inventory.Columns.Add(new DataGridViewImageColumn());
            inventory.Columns.Add(new DataGridViewTextBoxColumn());

            AvatarInventoryStyle.Apply(inventory);
            inventory.Columns.Add(new DataGridViewButtonColumn());

            ThemePalette palette = ThemePalette.For(mode);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.BackgroundColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.DefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.RowsDefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.AlternatingRowsDefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.Columns[0].DefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarItemBackground, inventory.Columns[1].DefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.Columns[2].DefaultCellStyle.BackColor);
            Assert.Equal(palette.AvatarCanvasBackground, inventory.Columns[3].DefaultCellStyle.BackColor);
            Assert.Equal(
                FlatStyle.Flat,
                ((DataGridViewButtonColumn)inventory.Columns[3]).FlatStyle);

            ThemeMode opposite = mode == ThemeMode.Dark ? ThemeMode.Light : ThemeMode.Dark;
            ThemeManager.Initialize(opposite);
            ThemeManager.Apply(inventory);
            ThemePalette oppositePalette = ThemePalette.For(opposite);
            Assert.Equal(oppositePalette.AvatarCanvasBackground, inventory.DefaultCellStyle.BackColor);
            Assert.Equal(oppositePalette.AvatarItemBackground, inventory.Columns[1].DefaultCellStyle.BackColor);
        });
    }
}
