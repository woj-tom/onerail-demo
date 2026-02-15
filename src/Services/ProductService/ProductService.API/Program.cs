using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductService.API;
using ProductService.API.Contracts;
using ProductService.API.Middlewares;
using ProductService.API.Validators;
using ProductService.Application.Handlers;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Database;
using ProductService.Infrastructure.Database.Repositories;
using ProductService.Infrastructure.Extensions;
using Serilog;
using Shared.Contracts.Repositories;
using Shared.Utils;
using Shared.Utils.Extensions;
using Shared.Utils.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ToDo: This should be moved to a configuration file
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] " +
        "[CorrelationId: {CorrelationId}] " +
        "[TraceId: {TraceId}] " +
        "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCustomAuth(builder.Configuration);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddControllers();

builder.Services.AddScoped<ProductCreateHandler>();
builder.Services.AddScoped<ProductListHandler>();
builder.Services.AddScoped<ProductInventoryAddedHandler>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProcessedMessageRepository, ProcessedMessageRepository>();

builder.Services.AddMessaging();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped<IValidator<ProductCreateReq>, ProductCreateReqValidator>();


var app = builder.Build();

app.UseMiddleware<StructuredLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
