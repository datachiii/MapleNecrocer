namespace MapleNecrocer;

internal static class AvatarCashFilter
{
    internal static bool ShouldInclude(bool cashOnly, int? cashValue)
    {
        return !cashOnly || cashValue.GetValueOrDefault() != 0;
    }

    internal static bool ShouldIncludeAvatarItem(
        bool cashOnly,
        int? cashValue,
        string? itemId)
    {
        return AvatarGenderFilter.IsAppearancePart(itemId)
            || ShouldInclude(cashOnly, cashValue);
    }
}
