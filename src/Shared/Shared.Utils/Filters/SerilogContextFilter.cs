using MassTransit;

namespace Shared.Utils.Filters;

public class SerilogContextFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        using (Serilog.Context.LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (Serilog.Context.LogContext.PushProperty("MessageId", context.MessageId))
        using (Serilog.Context.LogContext.PushProperty("ConversationId", context.ConversationId))
        {
            await next.Send(context);
        }
    }

    public void Probe(ProbeContext context) { }
}