using Microsoft.EntityFrameworkCore;
using ProductService.Application.Handlers;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Database;
using ProductService.Infrastructure.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ProductCreateHandler>();
builder.Services.AddScoped<ProductListHandler>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
