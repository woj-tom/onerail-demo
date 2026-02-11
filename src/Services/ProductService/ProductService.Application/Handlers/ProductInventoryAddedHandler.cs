using ProductService.Domain.Repositories;
using Shared.Contracts;

namespace ProductService.Application.Handlers;

public class ProductInventoryAddedHandler(IProductRepository repository)
{
    public async Task HandleAsync(ProductInventoryAddedEvent @event, CancellationToken ct)
    {
        var entry = await repository.GetAsync(@event.ProductId, ct);
        if (entry is null)
        {
            // ToDo: Do something smarter here
            throw new Exception($"Product {@event.ProductId} does not exist");
        }
        
        entry.IncreaseStock(@event.Quantity);
        await repository.SaveChangesAsync(ct);
    }
}