using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Infrastructure.Database.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task CreateAsync(Product product, CancellationToken ct)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Product>> ListAsync(CancellationToken ct)
    {
        return await db.Products.ToListAsync(ct);
    }
}