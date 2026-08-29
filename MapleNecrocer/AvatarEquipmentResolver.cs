using WzComparerR2.WzLib;

namespace MapleNecrocer;

internal sealed record AvatarEquipmentResolution(
    string EquipmentId,
    string WzPath,
    bool IsAvailable);

internal sealed class AvatarEquipmentResolver
{
    private readonly Func<string, Wz_Node?> findNode;

    internal AvatarEquipmentResolver(Func<string, Wz_Node?> findNode)
    {
        this.findNode = findNode;
    }

    internal AvatarEquipmentResolution Resolve(string equipmentId)
    {
        string wzPath = $"Character/{Equip.GetDir(equipmentId)}{equipmentId}.img";
        return new AvatarEquipmentResolution(
            equipmentId,
            wzPath,
            findNode(wzPath) is not null);
    }
}
