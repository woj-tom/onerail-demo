using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using Shared.Contracts;

namespace InventoryService.Application.Handlers;

public class ProductAddedHandler(IProductRepository repository)
{
    public async Task HandleAsync(ProductAddedEvent @event, CancellationToken ct)
    {
        var existing = await repository.GetAsync(@event.ProductId, ct);
        if (existing is not null) return;

        await repository.CreateAsync(new RegisteredProduct(
            @event.ProductId,
            @event.ProductName), ct);
    }
}