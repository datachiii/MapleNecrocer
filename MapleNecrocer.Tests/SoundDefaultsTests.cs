using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class SoundDefaultsTests
{
    [Fact]
    public void Sound_StartsMuted()
    {
        Assert.True(Sound.isMute);
    }
}
