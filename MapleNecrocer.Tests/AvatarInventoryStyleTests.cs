using MapleNecrocer;
using System.Drawing;
using System.Windows.Forms;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarInventoryStyleTests
{
    [Fact]
    public void Apply_UsesTheAvatarListPaletteForInventory()
    {
        using var inventory = new DataGridView();

        AvatarInventoryStyle.Apply(inventory);

        Assert.Equal(AvatarItemPalette.CanvasBackground, inventory.BackgroundColor);
        Assert.Equal(AvatarItemPalette.ItemBackground, inventory.DefaultCellStyle.BackColor);
        Assert.Equal(AvatarItemPalette.ItemBackground, inventory.RowsDefaultCellStyle.BackColor);
        Assert.Equal(AvatarItemPalette.ItemBackground, inventory.AlternatingRowsDefaultCellStyle.BackColor);
        Assert.Equal(AvatarItemPalette.ItemBackground, inventory.DefaultCellStyle.SelectionBackColor);
        Assert.Equal(Color.Black, inventory.DefaultCellStyle.SelectionForeColor);
    }
}
