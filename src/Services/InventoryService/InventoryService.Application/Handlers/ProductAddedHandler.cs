using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Contracts;
using Shared.Contracts.Models;
using Shared.Contracts.Repositories;

namespace InventoryService.Application.Handlers;

public class ProductAddedHandler(
    IProductRepository productRepository,
    IProcessedMessageRepository  processedMessageRepository,
    ITransactionManager context,
    ILogger<ProductAddedHandler> logger)
{
    public async Task HandleAsync(ProductAddedEvent @event, CancellationToken ct)
    {
        logger.LogInformation($"Handling product added event");
        if (await processedMessageRepository.ExistsAsync(@event.EventId, ct)) return;
        
        var existing = await productRepository.GetAsync(@event.ProductId, ct);
        if (existing is not null) return;

        await using var transaction = await  context.BeginTransactionAsync(ct);
        try
        {
            productRepository.Insert(new RegisteredProduct(
                @event.ProductId,
                @event.ProductName), ct);

            await processedMessageRepository.CreateAsync(new ProcessedMessage
            {
                MessageId = @event.EventId,
                MessageType = @event.GetType().Name,
                OccurredAt = @event.OccurredAt,
                ProcessedAt = DateTime.UtcNow
            }, ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation($"Stored new registered product {@event.ProductId}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}