namespace Catalog.Infrastructure.IntegrationEvents;

public class RecipetCreatedEvent
{
    public ICollection<RecieptItem> Details { get; set; } = null!;
}

public class RecieptItem
{
    public required string Slug { get; set; }
    public int Stock { get; set; }
}