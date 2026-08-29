namespace MapleNecrocer;

internal static class MusicPlaybackPolicy
{
    internal static bool ShouldAutoPlay(bool isMuted)
    {
        return !isMuted;
    }
}
