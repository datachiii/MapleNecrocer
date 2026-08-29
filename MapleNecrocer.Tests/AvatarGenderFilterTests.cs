using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarGenderFilterTests
{
    [Theory]
    [InlineData("All", "01040000", true)]
    [InlineData("All", "01041000", true)]
    [InlineData("Male", "01040000", true)]
    [InlineData("Male", "01041000", false)]
    [InlineData("Male", "01042000", true)]
    [InlineData("Female", "01040000", false)]
    [InlineData("Female", "01041000", true)]
    [InlineData("Female", "01042000", true)]
    public void ShouldInclude_AppliesGeneralEquipmentGender(
        string selectionName,
        string itemId,
        bool expected)
    {
        AvatarGenderSelection selection = Enum.Parse<AvatarGenderSelection>(selectionName);
        Assert.Equal(expected, AvatarGenderFilter.ShouldInclude(selection, itemId));
    }

    [Theory]
    [InlineData("00030000", "Male")]
    [InlineData("00031000", "Female")]
    [InlineData("00032000", "Unisex")]
    [InlineData("00033000", "Male")]
    [InlineData("00034000", "Female")]
    [InlineData("00035000", "Male")]
    [InlineData("00066000", "Male")]
    [InlineData("00067000", "Female")]
    [InlineData("00068187", "Female")]
    [InlineData("00069000", "Unisex")]
    public void Classify_UsesHairGenderTags(
        string itemId,
        string expectedName)
    {
        AvatarItemGender expected = Enum.Parse<AvatarItemGender>(expectedName);
        Assert.Equal(expected, AvatarGenderFilter.Classify(itemId));
    }

    [Theory]
    [InlineData("00020000", "Male")]
    [InlineData("00021000", "Female")]
    [InlineData("00022000", "Unisex")]
    [InlineData("00023000", "Male")]
    [InlineData("00054018", "Female")]
    [InlineData("00055000", "Male")]
    [InlineData("00056000", "Female")]
    [InlineData("00057000", "Male")]
    [InlineData("00058000", "Female")]
    [InlineData("00059000", "Unisex")]
    public void Classify_UsesFaceGenderTags(
        string itemId,
        string expectedName)
    {
        AvatarItemGender expected = Enum.Parse<AvatarItemGender>(expectedName);
        Assert.Equal(expected, AvatarGenderFilter.Classify(itemId));
    }

    [Theory]
    [InlineData("00002012")]
    [InlineData("00012012")]
    [InlineData("")]
    [InlineData("invalid")]
    public void Classify_TreatsBaseOrInvalidIdsAsUnisex(string itemId)
    {
        Assert.Equal(AvatarItemGender.Unisex, AvatarGenderFilter.Classify(itemId));
    }
}
