namespace MapleNecrocer;

internal enum AvatarSearchCategory
{
    All,
    Hair,
    Face,
    Cap,
    Coat,
    Pants,
    Longcoat,
    Cape,
    Shoes,
    Glove,
    Glass,
    Weapon,
    FaceAcc,
}

internal sealed record AvatarSearchItem(
    string ItemId,
    string Name,
    int? CashValue,
    PartName Category = PartName.Body);

internal static class AvatarSearchResultFilter
{
    internal static bool ShouldInclude(
        AvatarSearchItem item,
        bool cashOnly,
        AvatarGenderSelection gender,
        AvatarSearchCategory category = AvatarSearchCategory.All)
    {
        return AvatarCashFilter.ShouldIncludeAvatarItem(cashOnly, item.CashValue, item.ItemId)
               && AvatarGenderFilter.ShouldInclude(gender, item.ItemId)
               && MatchesCategory(item.Category, category);
    }

    private static bool MatchesCategory(PartName itemCategory, AvatarSearchCategory category)
    {
        return category switch
        {
            AvatarSearchCategory.All => true,
            AvatarSearchCategory.Hair => itemCategory == PartName.Hair,
            AvatarSearchCategory.Face => itemCategory == PartName.Face,
            AvatarSearchCategory.Cap => itemCategory == PartName.Cap,
            AvatarSearchCategory.Coat => itemCategory == PartName.Coat,
            AvatarSearchCategory.Pants => itemCategory == PartName.Pants,
            AvatarSearchCategory.Longcoat => itemCategory == PartName.Longcoat,
            AvatarSearchCategory.Cape => itemCategory == PartName.Cape,
            AvatarSearchCategory.Shoes => itemCategory == PartName.Shoes,
            AvatarSearchCategory.Glove => itemCategory == PartName.Glove,
            AvatarSearchCategory.Glass => itemCategory == PartName.Glass,
            AvatarSearchCategory.Weapon => itemCategory == PartName.Weapon,
            AvatarSearchCategory.FaceAcc => itemCategory == PartName.FaceAcc,
            _ => false,
        };
    }
}
