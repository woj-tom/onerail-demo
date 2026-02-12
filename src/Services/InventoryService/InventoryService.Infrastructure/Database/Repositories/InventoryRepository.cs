using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Infrastructure.Database.Repositories;

public class InventoryRepository(AppDbContext db) : IInventoryRepository
{
    public async Task InsertAsync(InventoryEntry entry, CancellationToken ct)
    {
        db.InventoryEntries.Add(entry);
        await db.SaveChangesAsync(ct);
    }

}