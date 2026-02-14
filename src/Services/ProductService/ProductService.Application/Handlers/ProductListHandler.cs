using Microsoft.Extensions.Logging;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class ProductListHandler(
    IProductRepository repository,
    ILogger<ProductListHandler> logger)
{
    public async Task<IReadOnlyList<ProductDto>> HandleAsync(CancellationToken ct)
    {
        var products = await repository.ListAsync(ct);
        logger.LogInformation("Retrieving list of products from database");
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