namespace MapleNecrocer;

internal readonly record struct AvatarAnimationFrameDelay(int Frame, int Delay);

internal static class AvatarAnimationFramePolicy
{
    internal static int GetMaximumFrame(IEnumerable<string> nodeNames)
    {
        int maximumFrame = 0;
        foreach (string nodeName in nodeNames)
        {
            if (int.TryParse(nodeName, out int frame) && frame >= 0)
            {
                maximumFrame = Math.Max(maximumFrame, frame);
            }
        }

        return maximumFrame;
    }

    internal static AvatarAnimationFrameDelay ResolveFrameDelay(
        IReadOnlyDictionary<string, int> data,
        string part,
        string action,
        int requestedFrame)
    {
        if (TryGetDelay(data, part, action, requestedFrame, out int delay))
        {
            return new AvatarAnimationFrameDelay(requestedFrame, delay);
        }

        if (requestedFrame != 0 && TryGetDelay(data, part, action, 0, out delay))
        {
            return new AvatarAnimationFrameDelay(0, delay);
        }

        return new AvatarAnimationFrameDelay(0, 0);
    }

    private static bool TryGetDelay(
        IReadOnlyDictionary<string, int> data,
        string part,
        string action,
        int frame,
        out int delay)
    {
        return data.TryGetValue($"{part}/{action}/{frame}/delay", out delay);
    }
}
