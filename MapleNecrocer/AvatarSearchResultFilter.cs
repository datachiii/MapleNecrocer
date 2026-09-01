namespace MapleNecrocer;

internal sealed record AvatarSearchItem(string ItemId, string Name, int? CashValue);

internal static class AvatarSearchResultFilter
{
    internal static bool ShouldInclude(
        AvatarSearchItem item,
        bool cashOnly,
        AvatarGenderSelection gender)
    {
        return AvatarCashFilter.ShouldIncludeAvatarItem(cashOnly, item.CashValue, item.ItemId)
               && AvatarGenderFilter.ShouldInclude(gender, item.ItemId);
    }
}
