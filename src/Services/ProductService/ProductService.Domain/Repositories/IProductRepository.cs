using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories;

public interface IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken ct);
    
    public Task<IEnumerable<Product>> ListAsync(CancellationToken ct);
}