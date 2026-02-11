using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class ProductListHandler(IProductRepository repository)
{
    public async Task<IReadOnlyList<ProductDto>> HandleAsync(CancellationToken ct)
    {
        var products = await repository.ListAsync(ct);
        return products
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Amount))
            .ToList();
    }
}

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Amount);