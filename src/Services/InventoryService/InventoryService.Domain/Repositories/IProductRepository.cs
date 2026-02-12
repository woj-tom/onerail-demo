using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories;

public interface IProductRepository
{
    public Task CreateAsync(RegisteredProduct product, CancellationToken ct);
    
    public Task<RegisteredProduct?> GetAsync(Guid id, CancellationToken ct);
}