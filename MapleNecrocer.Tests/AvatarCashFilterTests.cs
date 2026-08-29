using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarCashFilterTests
{
    [Theory]
    [InlineData(true, 1, true)]
    [InlineData(true, 0, false)]
    [InlineData(true, null, false)]
    [InlineData(false, 0, true)]
    [InlineData(false, null, true)]
    public void ShouldInclude_AppliesCashOnlyRule(
        bool cashOnly,
        int? cashValue,
        bool expected)
    {
        Assert.Equal(expected, AvatarCashFilter.ShouldInclude(cashOnly, cashValue));
    }

    [Theory]
    [InlineData(true, null, "00002012", true)]
    [InlineData(true, null, "00012012", true)]
    [InlineData(true, null, "00068187", true)]
    [InlineData(true, null, "00054018", true)]
    [InlineData(true, 0, "01040000", false)]
    [InlineData(true, 1, "01040000", true)]
    [InlineData(false, null, "01040000", true)]
    public void ShouldIncludeAvatarItem_BypassesCashForAppearanceParts(
        bool cashOnly,
        int? cashValue,
        string itemId,
        bool expected)
    {
        Assert.Equal(
            expected,
            AvatarCashFilter.ShouldIncludeAvatarItem(cashOnly, cashValue, itemId));
    }
}
