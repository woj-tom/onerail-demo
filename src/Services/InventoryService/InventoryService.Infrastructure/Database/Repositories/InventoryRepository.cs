using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Infrastructure.Database.Repositories;

public class InventoryRepository(AppDbContext db) : IInventoryRepository
{
    public void Insert(InventoryEntry entry, CancellationToken ct)
    {
        db.InventoryEntries.Add(entry);
    }

}