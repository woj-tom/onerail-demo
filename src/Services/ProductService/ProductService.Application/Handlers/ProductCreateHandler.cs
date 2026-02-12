using MassTransit;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using Shared.Contracts;

namespace ProductService.Application.Handlers;

public class ProductCreateHandler(
    IProductRepository repository,
    IPublishEndpoint publishEndpoint)
{
    public async Task HandleAsync(ProductCreateCommand command, CancellationToken ct)
    {
        var product = new Product(command.Name, command.Description, command.Price);
        await repository.CreateAsync(product, ct);
        await publishEndpoint.Publish(new ProductAddedEvent(
            Guid.NewGuid(),
            product.Id,
            product.Name,
            DateTime.UtcNow), ct);
    }
}

public sealed record ProductCreateCommand(string Name, string Description, decimal Price);