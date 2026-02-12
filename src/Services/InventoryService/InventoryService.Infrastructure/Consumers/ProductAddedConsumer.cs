using InventoryService.Application.Handlers;
using MassTransit;
using Shared.Contracts;

namespace InventoryService.Infrastructure.Consumers;

public class ProductAddedConsumer(ProductAddedHandler handler) : IConsumer<ProductAddedEvent>
{
    public async Task Consume(ConsumeContext<ProductAddedEvent> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}