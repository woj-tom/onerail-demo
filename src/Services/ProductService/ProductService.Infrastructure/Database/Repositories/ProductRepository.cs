using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Infrastructure.Database.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task InsertAsync(Product product, CancellationToken ct)
    {
        await db.Products.AddAsync(product, ct);
    }

    public async Task<IEnumerable<Product>> ListAsync(CancellationToken ct)
    {
        return await db.Products.ToListAsync(ct);
    }
    
    public async Task<Product?> GetAsync(Guid id, CancellationToken ct)
    {
        return await db.Products.FindAsync(id, ct);
    }
}