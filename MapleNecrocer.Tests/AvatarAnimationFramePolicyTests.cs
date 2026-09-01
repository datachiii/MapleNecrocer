using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class AvatarAnimationFramePolicyTests
{
    [Theory]
    [InlineData(new[] { "0", "1", "2", "info" }, 2)]
    [InlineData(new[] { "info", "meta" }, 0)]
    [InlineData(new[] { "0", "3" }, 3)]
    public void GetMaximumFrame_IgnoresNonNumericNodes(
        string[] nodeNames,
        int expectedMaximumFrame)
    {
        Assert.Equal(expectedMaximumFrame, AvatarAnimationFramePolicy.GetMaximumFrame(nodeNames));
    }

    [Fact]
    public void ResolveFrameDelay_UsesFrameZeroWhenRequestedFrameHasNoDelay()
    {
        var data = new Dictionary<string, int>
        {
            ["body/stand1/0/delay"] = 120,
            ["body/stand1/1/delay"] = 120,
            ["body/stand1/2/delay"] = 120,
        };

        var result = AvatarAnimationFramePolicy.ResolveFrameDelay(
            data,
            "body",
            "stand1",
            requestedFrame: 3);

        Assert.Equal(0, result.Frame);
        Assert.Equal(120, result.Delay);
    }
}
