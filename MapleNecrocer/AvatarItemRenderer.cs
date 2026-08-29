using System.Drawing;
using Manina.Windows.Forms;

namespace MapleNecrocer;

[Flags]
internal enum AvatarItemBorderState
{
    None = 0,
    Equipped = 1,
    Selected = 2,
}

internal static class AvatarItemBorderPolicy
{
    internal static AvatarItemBorderState Resolve(
        AvatarEquippedItemLookup equippedItems,
        string? itemId,
        bool selected)
    {
        AvatarItemBorderState borderState = AvatarItemBorderState.None;
        if (equippedItems.IsEquipped(itemId))
        {
            borderState |= AvatarItemBorderState.Equipped;
        }

        if (selected)
        {
            borderState |= AvatarItemBorderState.Selected;
        }

        return borderState;
    }
}

internal sealed class AvatarItemRenderer : ImageListView.ImageListViewRenderer
{
    private readonly AvatarEquippedItemLookup equippedItems;

    internal AvatarItemRenderer(AvatarEquippedItemLookup equippedItems)
    {
        this.equippedItems = equippedItems;
    }

    public override void DrawItem(
        Graphics graphics,
        ImageListViewItem item,
        ItemState state,
        Rectangle bounds)
    {
        base.DrawItem(graphics, item, state, bounds);

        AvatarItemBorderState borderState = AvatarItemBorderPolicy.Resolve(
            equippedItems,
            item.FileName,
            state.HasFlag(ItemState.Selected));
        if (!borderState.HasFlag(AvatarItemBorderState.Equipped))
        {
            return;
        }

        Rectangle equippedBounds = Rectangle.Inflate(bounds, -3, -3);
        using (var outlinePen = new Pen(AvatarItemPalette.EquippedOutline, 6f))
        {
            graphics.DrawRectangle(outlinePen, equippedBounds);
        }

        using (var equippedPen = new Pen(AvatarItemPalette.EquippedBorder, 4f))
        {
            graphics.DrawRectangle(equippedPen, equippedBounds);
        }

        if (borderState.HasFlag(AvatarItemBorderState.Selected))
        {
            using var selectedPen = new Pen(AvatarItemPalette.SelectedBorder, 2f);
            Rectangle selectedBounds = Rectangle.Inflate(bounds, -8, -8);
            graphics.DrawRectangle(selectedPen, selectedBounds);
        }
    }
}
