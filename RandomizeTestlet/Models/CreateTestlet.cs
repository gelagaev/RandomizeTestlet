using RandomizeTestlet.Extensions;

namespace RandomizeTestlet.Models;

/// <summary>
/// I think it's not the best idea to create an validate Extension for List&lt;Item&gt;,
/// so I created a model for creating <see cref="Testlet"/> that will handle the validation.
/// Although I could have created a Validate method directly in the this model,
/// I created an Extension <see cref="CreateTestletExtensions"/>.
/// </summary>
internal sealed class CreateTestlet
{
    public string TestletId { get; init; } = default!;
    public List<Item> Items { get; init; } = default!;
}