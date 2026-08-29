using WzComparerR2.WzLib;

namespace MapleNecrocer;

internal sealed class AvatarPartNodeResolver
{
    private readonly Func<string, Wz_Node?> findNode;
    private readonly Action<string> reportMissingNode;

    internal AvatarPartNodeResolver(
        Func<string, Wz_Node?> findNode,
        Action<string> reportMissingNode)
    {
        this.findNode = findNode;
        this.reportMissingNode = reportMissingNode;
    }

    internal IEnumerable<Wz_Node> GetChildren(string path)
    {
        Wz_Node? node = findNode(path);
        if (node is not null)
        {
            return node.Nodes;
        }

        reportMissingNode(path);
        return Enumerable.Empty<Wz_Node>();
    }
}
