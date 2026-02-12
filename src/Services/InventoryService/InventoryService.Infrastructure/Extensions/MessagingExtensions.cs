using InventoryService.Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Utils;

namespace InventoryService.Infrastructure.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        // Ref: https://masstransit.io/documentation/configuration/transports/rabbitmq#minimal-example
        services.AddMassTransit(x =>
        {
            // Ref: https://masstransit.io/documentation/configuration/consumers
            x.AddConsumer<ProductAddedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context
                    .GetRequiredService<IOptions<RabbitMqOptions>>()
                    .Value;
                
                cfg.Host(options.Host, options.Port, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}