using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductService.Infrastructure.Consumers;
using Shared.Utils;
using Shared.Utils.Filters;

namespace ProductService.Infrastructure.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        // Ref: https://masstransit.io/documentation/configuration/transports/rabbitmq#minimal-example
        services.AddMassTransit(x =>
        {
            // Ref: https://masstransit.io/documentation/configuration/consumers
            x.AddConsumer<ProductInventoryAddedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context
                    .GetRequiredService<IOptions<RabbitMqOptions>>()
                    .Value;
                
                // Ref: https://masstransit.io/documentation/configuration/middleware/scoped#useconsumefilter
                cfg.UseConsumeFilter(typeof(SerilogContextFilter<>), context);
                
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