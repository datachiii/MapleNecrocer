using System.Windows.Forms;
using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarGenderInputPolicyTests
{
    [Theory]
    [InlineData(Keys.Left, true)]
    [InlineData(Keys.Right, true)]
    [InlineData(Keys.Up, false)]
    [InlineData(Keys.Down, false)]
    [InlineData(Keys.Enter, false)]
    public void ShouldSuppressComboBoxNavigation_OnlySuppressesHorizontalArrows(
        Keys key,
        bool expected)
    {
        Assert.Equal(expected, AvatarGenderInputPolicy.ShouldSuppressComboBoxNavigation(key));
    }
}
