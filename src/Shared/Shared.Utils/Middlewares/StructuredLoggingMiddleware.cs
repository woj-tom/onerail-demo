using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace Shared.Utils.Middlewares;

public class StructuredLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationHeader = "X-Correlation-Id";
    
    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationHeader]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = context.TraceIdentifier;
        }

        context.Response.Headers[CorrelationHeader] = correlationId;
        
        var stopwatch = Stopwatch.StartNew();

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
        // Other things can be logged as well    
        // using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        // using (LogContext.PushProperty("RequestPath", context.Request.Path))
        // using (LogContext.PushProperty("QueryString", context.Request.QueryString.ToString()))
        {
            try
            {
                await next(context);
            }
            finally
            {
                stopwatch.Stop();

                Log.Information(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds
                );
            }
        }
    }
}