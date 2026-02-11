using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using MassTransit;
using Shared.Contracts;

namespace InventoryService.Application.Handlers;

public class InventoryCreateHandler(
    IInventoryRepository  repository,
    IPublishEndpoint publishEndpoint)
{
    public async Task HandleAsync(InventoryCreateCommand command, CancellationToken ct)
    {
        var entry = new InventoryEntry(command.ProductId, command.Quantity, command.AddedBy);
        await repository.InsertAsync(entry, ct);
        await publishEndpoint.Publish(new ProductInventoryAddedEvent
        {
            EventId = Guid.NewGuid(),
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            OccurredAt =  DateTime.UtcNow
        }, ct);
    }
}

public sealed record InventoryCreateCommand(Guid ProductId, int Quantity, string AddedBy);