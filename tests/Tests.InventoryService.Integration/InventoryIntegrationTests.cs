using System.Net;
using System.Net.Http.Json;
using DummyAuth.Core;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Database;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Testcontainers.PostgreSql;

namespace Tests.InventoryService.Integration;

[TestClass]
public sealed class InventoryIntegrationTests
{
    private static PostgreSqlContainer _postgres;

    // Ref: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=mstest
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    

    [ClassInitialize]
    public static async Task InitializeAsync(TestContext context)
    {
        _postgres = new PostgreSqlBuilder("postgres:17.7")
            .WithDatabase("inventory_db")
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
        
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add(
            "Authorization",
            [$"Bearer {TestAuthWrapper.GetToken()}"]);
    }
    
    [TestMethod]
    //                Who?             What?                    When?
    public async Task InventoryService_WhenCallingPOSTInventory_ShouldSendEvent()
    {
        // Arrange
        var productId = Guid.NewGuid();

        await using var scope = _factory.Services.CreateAsyncScope();
        
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.RegisteredProducts.Add(new RegisteredProduct(productId, "Product name"));
            await db.SaveChangesAsync();
        }

        var harness = _factory.Services.GetTestHarness();
        await harness.Start();

        // Act
        var response = await _client.PostAsJsonAsync("/inventory", new
        {
            ProductId = productId,
            Quantity = 10
        });
        
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsTrue(await harness.Published.Any<ProductInventoryAddedEvent>());
    }

    [TestCleanup]
    public async Task CleanupAsync()
    {
        await _postgres.StopAsync();
    }
}