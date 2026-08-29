namespace MapleNecrocer;

internal sealed class AvatarItemHistory
{
    private readonly int capacity;
    private readonly List<string> items = new();

    internal AvatarItemHistory(int capacity = 30)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        this.capacity = capacity;
    }

    internal IReadOnlyList<string> Items => items;

    internal void Add(string itemId)
    {
        items.Remove(itemId);
        items.Insert(0, itemId);

        if (items.Count > capacity)
        {
            items.RemoveAt(items.Count - 1);
        }
    }

    internal void Clear()
    {
        items.Clear();
    }
}
