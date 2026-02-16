using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories;

public interface IInventoryRepository
{
    void Insert(InventoryEntry entry, CancellationToken ct);
}