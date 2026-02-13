using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using Shared.Contracts;
using Shared.Contracts.Models;
using Shared.Contracts.Repositories;

namespace InventoryService.Application.Handlers;

public class ProductAddedHandler(
    IProductRepository productRepository,
    IProcessedMessageRepository  processedMessageRepository)
{
    public async Task HandleAsync(ProductAddedEvent @event, CancellationToken ct)
    {
        if (await processedMessageRepository.ExistsAsync(@event.EventId, ct)) return;
        
        var existing = await productRepository.GetAsync(@event.ProductId, ct);
        if (existing is not null) return;

        await productRepository.CreateAsync(new RegisteredProduct(
            @event.ProductId,
            @event.ProductName), ct);

        await processedMessageRepository.CreateAsync(new ProcessedMessage
        {
            MessageId = @event.EventId,
            MessageType = @event.GetType().Name,
            OccurredAt =  @event.OccurredAt,
            ProcessedAt = DateTime.UtcNow
        }, ct);
    }
}