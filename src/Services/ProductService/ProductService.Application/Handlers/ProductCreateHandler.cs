using MassTransit;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using Shared.Contracts;

namespace ProductService.Application.Handlers;

public class ProductCreateHandler(
    IProductRepository repository,
    IPublishEndpoint publishEndpoint,
    ITransactionManager context,
    ILogger<ProductCreateHandler> logger)
{
    public async Task<ProductDto> HandleAsync(ProductCreateCommand command, CancellationToken ct)
    {
        await using var transaction = await context.BeginTransactionAsync(ct);

        try
        {
            var product = new Product(command.Name, command.Description, command.Price);
            logger.LogInformation($"New product {command.Name} created");
            
            await repository.InsertAsync(product, ct);

            await publishEndpoint.Publish(new ProductAddedEvent(
                Guid.NewGuid(),
                product.Id,
                product.Name,
                DateTime.UtcNow), ct);
            logger.LogInformation($"Event {nameof(ProductAddedEvent)} published");
            
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation($"Product {command.Name} stored in database");

            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Amount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}

public sealed record ProductCreateCommand(string Name, string Description, decimal Price);