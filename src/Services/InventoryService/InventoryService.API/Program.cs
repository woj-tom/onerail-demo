using InventoryService.Application.Handlers;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.Database;
using InventoryService.Infrastructure.Database.Repositories;
using InventoryService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<InventoryCreateHandler>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
