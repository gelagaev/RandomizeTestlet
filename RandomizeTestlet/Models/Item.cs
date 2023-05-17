using RandomizeTestlet.Enums;

namespace RandomizeTestlet.Models;

internal sealed class Item
{
    public string ItemId { get; init; } = string.Empty;
    public ItemType ItemType { get; init; }

    public bool IsPretestItem()
    {
        return ItemType == ItemType.Pretest;
    }

    public bool IsOperationalItem()
    {
        return ItemType == ItemType.Operational;
    }
}