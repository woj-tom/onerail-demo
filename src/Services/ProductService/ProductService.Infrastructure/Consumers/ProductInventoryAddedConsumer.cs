using MassTransit;
using ProductService.Application.Handlers;
using Shared.Contracts;

namespace ProductService.Infrastructure.Consumers;

public class ProductInventoryAddedConsumer(ProductInventoryAddedHandler handler) : IConsumer<ProductInventoryAddedEvent>
{
    public async Task Consume(ConsumeContext<ProductInventoryAddedEvent> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}