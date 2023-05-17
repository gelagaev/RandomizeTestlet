using RandomizeTestlet.Extensions;

namespace RandomizeTestlet.Models;

internal sealed class Testlet
{
    public string TestletId { get; private set; }
    private readonly List<Item> _items;

    public Testlet(CreateTestlet createTestlet)
    {
        createTestlet.Validate();
        TestletId = createTestlet.TestletId;
        _items = createTestlet.Items;
    }

    public List<Item> Randomize()
    {
        const int preTestItemsCount = 2;
        var randomizedItems = new List<Item>(_items.Count);

        var pretestItems = _items.Where(item => item.IsPretestItem()).ToList();
        randomizedItems.AddRange(Shuffle(pretestItems).Take(preTestItemsCount));

        var itemsExceptAdded = _items.Except(randomizedItems).ToList();
        randomizedItems.AddRange(Shuffle(itemsExceptAdded));

        return randomizedItems;
    }

    private static List<Item> Shuffle(List<Item> items)
    {
        if (items.Count <= 1)
        {
            return items;
        }

        var random = new Random();
        var shuffledList = new List<Item>(items);

        var shufflingIndex = shuffledList.Count;
        while (shufflingIndex > 0)
        {
            shufflingIndex--;
            var tempItem = shuffledList[shufflingIndex];
            var randomIndex = random.Next(0, shufflingIndex);
            shuffledList[shufflingIndex] = shuffledList[randomIndex];
            shuffledList[randomIndex] = tempItem;
        }

        //There's a nonzero probability that after shuffling the items,
        //they remain in the same order, so we will perform a check, and if that's the case,
        //we will recursively initiate the shuffling again.
        //
        //The question of whether this check is necessary should have been addressed to the analyst or
        //raised during the code review.
        return shuffledList.SequenceEqual(items) ? Shuffle(items) : shuffledList;
    }
}