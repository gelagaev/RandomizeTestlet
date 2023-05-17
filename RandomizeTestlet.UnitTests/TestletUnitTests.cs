using Bogus;
using RandomizeTestlet.Constants;
using RandomizeTestlet.Enums;
using RandomizeTestlet.Models;

namespace RandomizeTestlet.UnitTests;

public sealed class TestletUnitTests
{
    private readonly Faker _faker;
    private const int PretestItemsCount = 4;
    private const int OperationalItemsCount = 6;

    public TestletUnitTests()
    {
        _faker = new Faker();
    }

    [Theory(DisplayName = "Should randomize Testlet items")]
    [InlineData(2)]
    internal void ShouldRandomizeTestletItems(int firstPretestItemsCount)
    {
        var testletId = _faker.Random.String2(10);
        var items = CreateItemsList(PretestItemsCount, OperationalItemsCount);
        var testlet = new Testlet(new CreateTestlet
        {
            TestletId = testletId,
            Items = items,
        });

        var actual = testlet.Randomize();
        var actualFirstItems = actual.Take(firstPretestItemsCount).ToList();
        var actualLastItems = actual.TakeLast(actual.Count - firstPretestItemsCount).ToList();
        var originallyOrderedFirstItems = items.Where(item => item.IsPretestItem()).Take(firstPretestItemsCount);
        var originallyOrderedLastItems = items.Except(actualFirstItems);

        //First items are Pretest
        Assert.True(actualFirstItems.All(item => item.IsPretestItem()));
        //First items are shuffled
        Assert.NotEqual(originallyOrderedFirstItems, actualFirstItems);
        //Last items are shuffled
        Assert.NotEqual(originallyOrderedLastItems, actualLastItems);
        //Actual items are the same, disregarding the order.
        Assert.Equivalent(items, actual, true);
    }

    [Theory(DisplayName = "Should throw ArgumentNullException on invalid TestletId field")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    internal void ShouldThrowArgumentNullExceptionOnInvalidTestletId(string testletId)
    {
        var createTestlet = new CreateTestlet
        {
            TestletId = testletId,
            Items = CreateItemsList(PretestItemsCount, OperationalItemsCount),
        };

        var exception = Assert.Throws<ArgumentNullException>(() => new Testlet(createTestlet));

        Assert.Equal($"Value cannot be null. (Parameter '{nameof(createTestlet.TestletId)}')", exception.Message);
    }

    [Theory(DisplayName = "Should throw ArgumentNullException on invalid ItemId field")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    internal void ShouldThrowArgumentNullExceptionOnInvalidItemId(string itemId)
    {
        var items = new List<Item>(PretestItemsCount + OperationalItemsCount);
        items.AddRange(CreateItems(ItemType.Pretest, PretestItemsCount));
        items.AddRange(CreateItems(ItemType.Operational, OperationalItemsCount - 1));
        items.Add(new Item
        {
            ItemId = itemId,
            ItemType = ItemType.Operational
        });
        var createTestlet = new CreateTestlet
        {
            TestletId = _faker.Random.String2(10),
            Items = items,
        };

        var exception = Assert.Throws<ArgumentNullException>(() => new Testlet(createTestlet));

        Assert.Equal($"Value cannot be null. (Parameter '{nameof(Item.ItemId)}')", exception.Message);
    }

    [Theory(DisplayName = "Should throw ArgumentOutOfRangeException on incorrect Pretest items count")]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(6)]
    internal void ShouldThrowArgumentOutOfRangeExceptionOnIncorrectPretestItemsCount(int pretestItemsCount)
    {
        var createTestlet = new CreateTestlet
        {
            TestletId = _faker.Random.String2(10),
            Items = CreateItemsList(pretestItemsCount, OperationalItemsCount),
        };

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Testlet(createTestlet));

        Assert.Equal(
            $"{string.Format(ValidationErrors.PretestItemsCountError, PretestItemsCount)} (Parameter '{nameof(createTestlet.Items)}')\nActual value was {pretestItemsCount}.",
            exception.Message);
    }

    [Theory(DisplayName = "Should throw ArgumentOutOfRangeException on incorrect Operational items count")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(8)]
    internal void ShouldThrowArgumentOutOfRangeExceptionOnIncorrectOperationalItemsCount(int operationalItemsCount)
    {
        var createTestlet = new CreateTestlet
        {
            TestletId = _faker.Random.String2(10),
            Items = CreateItemsList(PretestItemsCount, operationalItemsCount),
        };

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Testlet(createTestlet));

        Assert.Equal(
            $"{string.Format(ValidationErrors.OperationalItemsCountError, OperationalItemsCount)} (Parameter '{nameof(createTestlet.Items)}')\nActual value was {operationalItemsCount}.",
            exception.Message);
    }

    [Fact(DisplayName = "Should successfully create Testlet")]
    internal void ShouldCreateTestlet()
    {
        var testletId = _faker.Random.String2(10);
        var items = CreateItemsList(PretestItemsCount, OperationalItemsCount);

        var actual = new Testlet(new CreateTestlet
        {
            TestletId = testletId,
            Items = items,
        });

        Assert.Equal(testletId, actual.TestletId);
    }

    private List<Item> CreateItemsList(int pretestItemsCount, int operationalItemsCount)
    {
        var items = new List<Item>(pretestItemsCount + operationalItemsCount);
        items.AddRange(CreateItems(ItemType.Pretest, pretestItemsCount));
        items.AddRange(CreateItems(ItemType.Operational, operationalItemsCount));
        return items;
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