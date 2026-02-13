using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Database;
using Shared.Contracts;
using Testcontainers.PostgreSql;

namespace Tests.ProductService.Integration;

[TestClass]
public sealed class ProductIntegrationTests
{
    private static PostgreSqlContainer _postgres;

    // Ref: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=mstest
    private WebApplicationFactory<Program> _factory;
    

    [ClassInitialize]
    public static async Task InitializeAsync(TestContext context)
    {
        _postgres = new PostgreSqlBuilder("postgres:17.7")
            .WithDatabase("product_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
        
        await _postgres.StartAsync();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:Postgres"] = _postgres.GetConnectionString()
                    };

                    config.AddInMemoryCollection(dict!);
                });
                
                // Ref: https://masstransit.io/documentation/concepts/testing#web-application-factory
                builder.ConfigureServices(services =>
                {
                    services.AddMassTransitTestHarness();
                });
             
                builder.UseEnvironment("Development");
            });
    }

    [TestMethod]
    //                Who?           When?                       What?
    public async Task ProductService_WhenReceivedDuplicatedEvent_ShouldBeIdempotent()
    {
        // Arrange
        var product = new Product("Product A", "...", 20.0m);
        
        await using var scope = _factory.Services.CreateAsyncScope();
        var harness = scope.ServiceProvider.GetRequiredService<ITestHarness>();
        await harness.Start();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Products.AddAsync(product);
        await db.SaveChangesAsync();
        
        var @event = new ProductInventoryAddedEvent(
            Guid.NewGuid(),
            product.Id,
            10,
            DateTime.UtcNow);

        // Act
        await harness.Bus.Publish(@event);
        await harness.Bus.Publish(@event);
        await harness.Bus.Publish(@event);
        
        // Assert
        Assert.IsTrue(await harness.Consumed.Any<ProductInventoryAddedEvent>());
        Assert.IsTrue(await harness.Consumed.Any<ProductInventoryAddedEvent>());
        Assert.IsTrue(await harness.Consumed.Any<ProductInventoryAddedEvent>());
        
        db.ChangeTracker.Clear();
        product = await db.Products.FindAsync(product.Id);
        Assert.IsNotNull(product);
        Assert.AreEqual(10, product.Amount);
    }

    [TestCleanup]
    public async Task CleanupAsync()
    {
        await _postgres.StopAsync();
    }
}