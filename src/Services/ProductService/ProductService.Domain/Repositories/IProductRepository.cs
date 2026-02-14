using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories;

public interface IProductRepository
{
    public Task InsertAsync(Product product, CancellationToken ct);
    
    public Task<IEnumerable<Product>> ListAsync(CancellationToken ct);

    public Task<Product?> GetAsync(Guid id, CancellationToken ct);
    
    public Task SaveChangesAsync(CancellationToken ct);
}