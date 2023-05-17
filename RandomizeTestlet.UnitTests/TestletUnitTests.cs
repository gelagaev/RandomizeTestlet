using Bogus;
using RandomizeTestlet.Enums;
using RandomizeTestlet.Models;

namespace RandomizeTestlet.UnitTests;

public sealed class TestletUnitTests
{
    private readonly Testlet _testlet;
    private readonly List<Item> _items;
    private readonly Faker _faker;

    public TestletUnitTests()
    {
        _faker = new Faker();
        const int pretestItemsCount = 4;
        const int operationalItemsCount = 6;
        _items = new List<Item>(pretestItemsCount + operationalItemsCount);
        _items.AddRange(CreateItems(ItemType.Pretest, pretestItemsCount));
        _items.AddRange(CreateItems(ItemType.Operational, operationalItemsCount));
        _testlet = new Testlet(_faker.Random.String(), _items);
    }

    [Theory(DisplayName = "Should randomize Testlet items")]
    [InlineData(2)]
    internal void ShouldRandomizeTestletItems(int firstPretestItemsCount)
    {
        var actual = _testlet.Randomize();
        var actualFirstItems = actual.Take(firstPretestItemsCount).ToList();
        var actualLastItems = actual.TakeLast(actual.Count - firstPretestItemsCount).ToList();
        var originallyOrderedFirstItems = _items.Where(item => item.IsPretestItem()).Take(firstPretestItemsCount);
        var originallyOrderedLastItems = _items.Except(actualFirstItems);

        //First items are Pretest
        Assert.True(actualFirstItems.All(item => item.IsPretestItem()));
        //First items are shuffled
        Assert.NotEqual(originallyOrderedFirstItems, actualFirstItems);
        //Last items are shuffled
        Assert.NotEqual(originallyOrderedLastItems, actualLastItems);
        //Actual items are the same, disregarding the order.
        Assert.Equivalent(_items, actual, true);
    }

    private IEnumerable<Item> CreateItems(ItemType itemType, int count)
    {
        while (count > 0)
        {
            yield return new Item { ItemId = _faker.Random.String2(10), ItemType = itemType };
            count--;
        }
    }
}