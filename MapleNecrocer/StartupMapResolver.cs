namespace MapleNecrocer;

internal sealed record StartupMapResolution(string NodePath, string ResolvedMapId);

internal static class StartupMapResolver
{
    internal static StartupMapResolution? Resolve(
        string requestedMapId,
        bool hasMap9Dir,
        Func<string, bool> nodeExists,
        Func<string, string?> readLink)
    {
        if (string.IsNullOrWhiteSpace(requestedMapId))
        {
            return null;
        }

        string nodePath = hasMap9Dir
            ? $"Map/Map/Map{requestedMapId[0]}/{requestedMapId}.img"
            : $"Map/Map/{requestedMapId}.img";
        if (!nodeExists(nodePath))
        {
            return null;
        }

        string? linkedMapId = readLink(nodePath + "/info/link");
        string resolvedMapId = string.IsNullOrWhiteSpace(linkedMapId)
            ? requestedMapId
            : linkedMapId.PadLeft(9, '0');

        return new StartupMapResolution(nodePath, resolvedMapId);
    }
}
