using System.ComponentModel.DataAnnotations;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using MassTransit;
using Shared.Contracts;

namespace InventoryService.Application.Handlers;

public class InventoryCreateHandler(
    IInventoryRepository  inventoryRepository,
    IProductRepository  productRepository,
    IPublishEndpoint publishEndpoint)
{
    public async Task HandleAsync(InventoryCreateCommand command, CancellationToken ct)
    {
        var exists = await productRepository.GetAsync(command.ProductId, ct) is not null;
        if (!exists)
            throw new ValidationException("Product does not exist");
        
        var entry = new InventoryEntry(command.ProductId, command.Quantity, command.AddedBy);
        await inventoryRepository.InsertAsync(entry, ct);
        
        await publishEndpoint.Publish(new ProductInventoryAddedEvent(
            Guid.NewGuid(),
            command.ProductId,
            command.Quantity,
            DateTime.UtcNow
        ), ct);
    }
}

public sealed record InventoryCreateCommand(Guid ProductId, int Quantity, string AddedBy);