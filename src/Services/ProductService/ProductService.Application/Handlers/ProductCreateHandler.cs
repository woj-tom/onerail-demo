using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Handlers;

public class ProductCreateHandler(IProductRepository repository)
{
    public async Task HandleAsync(ProductCreateCommand command, CancellationToken ct)
    {
        var product = new Product(command.Name, command.Description, command.Price);
        await repository.CreateAsync(product, ct);
    }
}

public sealed record ProductCreateCommand(string Name, string Description, decimal Price);