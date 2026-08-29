using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class MusicPlaybackPolicyTests
{
    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void ShouldAutoPlay_IsTheInverseOfMute(bool isMuted, bool expected)
    {
        Assert.Equal(expected, MusicPlaybackPolicy.ShouldAutoPlay(isMuted));
    }
}
