using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Application.Handlers;

public class InventoryCreateHandler(IInventoryRepository  repository)
{
    public async Task Handle(InventoryCreateCommand command, CancellationToken ct)
    {
        var entry = new InventoryEntry(command.ProductId, command.Quantity, command.AddedBy);
        await repository.InsertAsync(entry, ct);
    }
}

public sealed record InventoryCreateCommand(Guid ProductId, int Quantity, string AddedBy);