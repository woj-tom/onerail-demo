using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    
    private const string ApplicationJsonHeader = "application/json";

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException validationException)
        {
            await HandleValidationException(context, validationException);
        }
        catch (Exception exception)
        {
            await HandleUnhandledException(context, exception);
        }
    }

    private async Task HandleValidationException(
        HttpContext context,
        ValidationException exception)
    {
        logger.LogWarning(exception,
            "Validation failure occurred.");

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = ApplicationJsonHeader;

        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problem = new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed"
        };

        foreach (var error in errors)
        {
            problem.Errors.Add(error.Key, error.Value);
        }

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problem);
    }

    private async Task HandleUnhandledException(
        HttpContext context,
        Exception exception)
    {
        logger.LogError(exception,
            "Unhandled exception occurred.");

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = ApplicationJsonHeader;

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error",
            Detail = environment.IsDevelopment()
                ? exception.Message
                : null
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problem);
    }
}