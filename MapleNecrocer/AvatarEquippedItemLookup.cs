namespace MapleNecrocer;

internal sealed class AvatarEquippedItemLookup
{
    private HashSet<string> equippedItemIds = new(StringComparer.Ordinal);

    internal void ReplaceWith(IEnumerable<string> itemIds)
    {
        equippedItemIds = new HashSet<string>(itemIds, StringComparer.Ordinal);
    }

    internal bool IsEquipped(string? itemId)
    {
        return itemId is not null && equippedItemIds.Contains(itemId);
    }
}
