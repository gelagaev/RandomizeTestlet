using RandomizeTestlet.Constants;
using RandomizeTestlet.Models;

namespace RandomizeTestlet.Extensions;

internal static class CreateTestletExtensions
{
    public static void Validate(this CreateTestlet testlet)
    {
        if (string.IsNullOrWhiteSpace(testlet.TestletId))
        {
            throw new ArgumentNullException(nameof(testlet.TestletId));
        }

        const int pretestItemsCount = 4;
        var actualPretestItemsCount = testlet.Items.Count(items => items.IsPretestItem());
        if (actualPretestItemsCount != pretestItemsCount)
        {
            throw new ArgumentOutOfRangeException(nameof(testlet.Items), actualPretestItemsCount,
                string.Format(ValidationErrors.PretestItemsCountError, pretestItemsCount));
        }

        const int operationalItemsCount = 6;
        var actualOperationalItemsCount = testlet.Items.Count(items => items.IsOperationalItem());
        if (actualOperationalItemsCount != operationalItemsCount)
        {
            throw new ArgumentOutOfRangeException(nameof(testlet.Items), actualOperationalItemsCount,
                string.Format(ValidationErrors.OperationalItemsCountError, operationalItemsCount));
        }

        if (testlet.Items.Any(item => string.IsNullOrWhiteSpace(item.ItemId)))
        {
            throw new ArgumentNullException(nameof(Item.ItemId));
        }
    }
}