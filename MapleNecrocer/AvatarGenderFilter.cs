using WzComparerR2.CharaSim;

namespace MapleNecrocer;

internal enum AvatarGenderSelection
{
    All,
    Male,
    Female
}

internal enum AvatarItemGender
{
    Male,
    Female,
    Unisex
}

internal static class AvatarGenderFilter
{
    internal static AvatarItemGender Classify(string? itemId)
    {
        if (!int.TryParse(itemId, out int id))
        {
            return AvatarItemGender.Unisex;
        }

        GearType type = Gear.GetGearType(id);
        if (type is GearType.body or GearType.head)
        {
            return AvatarItemGender.Unisex;
        }

        int tag = id / 1000 % 10;
        if (IsHairId(id))
        {
            return tag switch
            {
                0 or 3 or 5 or 6 => AvatarItemGender.Male,
                1 or 4 or 7 or 8 => AvatarItemGender.Female,
                _ => AvatarItemGender.Unisex
            };
        }

        if (IsFaceId(id))
        {
            return tag switch
            {
                0 or 3 or 5 or 7 => AvatarItemGender.Male,
                1 or 4 or 6 or 8 => AvatarItemGender.Female,
                _ => AvatarItemGender.Unisex
            };
        }

        return Gear.GetGender(id) switch
        {
            0 => AvatarItemGender.Male,
            1 => AvatarItemGender.Female,
            _ => AvatarItemGender.Unisex
        };
    }

    private static bool IsFaceId(int id)
    {
        return id is >= 20_000 and < 30_000
            or >= 50_000 and < 60_000
            or >= 80_000 and < 90_000;
    }

    private static bool IsHairId(int id)
    {
        return id is >= 30_000 and < 50_000
            or >= 60_000 and < 80_000
            or >= 90_000 and < 100_000;
    }

    internal static bool IsAppearancePart(string? itemId)
    {
        if (!int.TryParse(itemId, out int id))
        {
            return false;
        }

        GearType type = Gear.GetGearType(id);
        return type is GearType.body or GearType.head
            || IsFaceId(id)
            || IsHairId(id);
    }

    internal static bool ShouldInclude(
        AvatarGenderSelection selection,
        string? itemId)
    {
        AvatarItemGender gender = Classify(itemId);
        return selection switch
        {
            AvatarGenderSelection.Male => gender is not AvatarItemGender.Female,
            AvatarGenderSelection.Female => gender is not AvatarItemGender.Male,
            _ => true
        };
    }
}
