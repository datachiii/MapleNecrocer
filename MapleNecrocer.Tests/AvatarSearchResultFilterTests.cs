using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarSearchResultFilterTests
{
    [Theory]
    [InlineData(false, "All", 0, true)]
    [InlineData(true, "All", 1, true)]
    [InlineData(true, "All", null, false)]
    [InlineData(false, "Male", 0, true)]
    [InlineData(false, "Female", 0, false)]
    public void ShouldInclude_AppliesCachedCashAndGenderMetadata(
        bool cashOnly,
        string genderName,
        int? cashValue,
        bool expected)
    {
        var item = new AvatarSearchItem("01040000", "Test", cashValue);
        AvatarGenderSelection gender = Enum.Parse<AvatarGenderSelection>(genderName);

        Assert.Equal(expected, AvatarSearchResultFilter.ShouldInclude(item, cashOnly, gender));
    }

    [Theory]
    [InlineData("All", "Hair", true)]
    [InlineData("Hair", "Hair", true)]
    [InlineData("Face", "Hair", false)]
    [InlineData("FaceAcc", "FaceAcc", true)]
    public void ShouldInclude_AppliesSelectedCategory(
        string selectedCategoryName,
        string itemCategoryName,
        bool expected)
    {
        var item = new AvatarSearchItem(
            "01040000",
            "Test",
            CashValue: 1,
            Enum.Parse<PartName>(itemCategoryName));

        Assert.Equal(
            expected,
            AvatarSearchResultFilter.ShouldInclude(
                item,
                cashOnly: false,
                AvatarGenderSelection.All,
                Enum.Parse<AvatarSearchCategory>(selectedCategoryName)));
    }
}
