using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;

namespace InventoryService.Infrastructure.Database.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async void Insert(RegisteredProduct product, CancellationToken ct)
    {
        db.RegisteredProducts.Add(product);
    }

    public async Task<RegisteredProduct?> GetAsync(Guid id, CancellationToken ct)
    {
        return await db.RegisteredProducts.FindAsync(id, ct);
    }
}