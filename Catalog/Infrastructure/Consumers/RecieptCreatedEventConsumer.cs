
namespace Catalog.Infrastructure.Consumers
{
    public class RecieptCreatedEventConsumer(CatalogDbContext dbContext,ILogger<RecieptCreatedEventConsumer> logger)
        : IConsumer<RecipetCreatedEvent>
    {
        private readonly CatalogDbContext _dbContext = dbContext;
        private readonly ILogger<RecieptCreatedEventConsumer> _logger = logger;
        public async Task Consume(ConsumeContext<RecipetCreatedEvent> context)
        {
            foreach (var item in context.Message.Details)
            {
                var catalogItem = await _dbContext.CatalogItems.FirstOrDefaultAsync(x => x.Slug == item.Slug);
                if (catalogItem is null)
                {
                    _logger.LogWarning("Invalid Slug:{Slug}",item.Slug);
                    return;
                }
                catalogItem.AddStock(item.Stock);
            }
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
