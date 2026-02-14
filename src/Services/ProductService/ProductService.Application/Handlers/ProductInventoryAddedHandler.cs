using Microsoft.Extensions.Logging;
using ProductService.Domain.Repositories;
using Shared.Contracts;
using Shared.Contracts.Models;
using Shared.Contracts.Repositories;

namespace ProductService.Application.Handlers;

public class ProductInventoryAddedHandler(
    IProductRepository productRepository,
    IProcessedMessageRepository  processedMessageRepository,
    ILogger<ProductInventoryAddedHandler> logger)
{
    public async Task HandleAsync(ProductInventoryAddedEvent @event, CancellationToken ct)
    {
        logger.LogInformation($"Handling product inventory added event");
        if (await processedMessageRepository.ExistsAsync(@event.EventId, ct)) return;

        var entry = await productRepository.GetAsync(@event.ProductId, ct)
            ?? throw new NullReferenceException($"Product {@event.ProductId} does not exist");
        
        entry.IncreaseStock(@event.Quantity);
        
        await processedMessageRepository.CreateAsync(new ProcessedMessage
        {
            MessageId = @event.EventId,
            MessageType = @event.GetType().Name,
            OccurredAt =  @event.OccurredAt,
            ProcessedAt = DateTime.UtcNow
        }, ct);
        
        await productRepository.SaveChangesAsync(ct);
        logger.LogInformation($"Increased stock of product {@event.ProductId}");
    }
}