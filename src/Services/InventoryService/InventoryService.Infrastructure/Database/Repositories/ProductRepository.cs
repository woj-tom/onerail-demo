using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Infrastructure.Database.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task InsertAsync(RegisteredProduct product, CancellationToken ct)
    {
        await db.RegisteredProducts.AddAsync(product, ct);
    }

    public async Task<RegisteredProduct?> GetAsync(Guid id, CancellationToken ct)
    {
        return await db.RegisteredProducts.FindAsync(id, ct);
    }
}