using FluentValidation;
using InventoryService.API.Contracts;
using InventoryService.API.Middlewares;
using InventoryService.API.Validators;
using InventoryService.Application.Handlers;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Database;
using InventoryService.Infrastructure.Database.Repositories;
using InventoryService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Contracts;
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

builder.Services.AddScoped<InventoryCreateHandler>();
builder.Services.AddScoped<ProductAddedHandler>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProcessedMessageRepository, ProcessedMessageRepository>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

builder.Services.AddMessaging();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped<IValidator<InventoryCreateReq>, InventoryCreateReqValidator>();

var app = builder.Build();

app.UseMiddleware<StructuredLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
