using System.ComponentModel.DataAnnotations;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace InventoryService.Application.Handlers;

public class InventoryCreateHandler(
    IInventoryRepository  inventoryRepository,
    IProductRepository  productRepository,
    IPublishEndpoint publishEndpoint,
    ITransactionManager context,
    ILogger<InventoryCreateHandler> logger)
{
    public async Task HandleAsync(
        InventoryCreateCommand command,
        CancellationToken ct)
    {
        var exists = await productRepository.GetAsync(command.ProductId, ct) is not null;
        if (!exists)
            throw new ValidationException("Product does not exist");
        
        await using var transaction = await context.BeginTransactionAsync(ct);
        try
        {
            var entry = new InventoryEntry(command.ProductId, command.Quantity, command.AddedBy);
            logger.LogInformation($"New inventory entry {entry.Id} created");
            await inventoryRepository.InsertAsync(entry, ct);

            await publishEndpoint.Publish(new ProductInventoryAddedEvent(
                Guid.NewGuid(),
                command.ProductId,
                command.Quantity,
                DateTime.UtcNow
            ), ct);
            logger.LogInformation($"Event {nameof(ProductInventoryAddedEvent)} published");

            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation($"Inventory entry {entry.Id} stored in database");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}

public sealed record InventoryCreateCommand(Guid ProductId, int Quantity, string AddedBy);