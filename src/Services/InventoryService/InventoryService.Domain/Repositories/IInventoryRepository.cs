using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories;

public interface IInventoryRepository
{
    Task InsertAsync(InventoryEntry entry, CancellationToken ct);
}