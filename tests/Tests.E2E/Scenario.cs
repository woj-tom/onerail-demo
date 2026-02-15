using System.Net.Http.Json;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DummyAuth.Core;
using InventoryService.API.Contracts;
using ProductService.API.Contracts;
using ProductService.Application.Handlers;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Tests.E2E;

[TestClass]
public sealed class E2EScenario
{
    private static PostgreSqlContainer _postgresInventory;
    private static PostgreSqlContainer _postgresProduct;
    private static RabbitMqContainer _rabbitMq;
    private static IContainer _inventorySvc;
    private static IContainer _productSvc;

    private HttpClient _client;
    
    [ClassInitialize]
    public static async Task InitializeAsync(TestContext context)
    {
        var network = new NetworkBuilder().Build();
        
        _postgresInventory = new PostgreSqlBuilder("postgres:17.7")
            .WithDatabase("inventory_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithNetwork(network)
            .WithNetworkAliases("postgres-inventory")
            .Build();
        
        _postgresProduct = new PostgreSqlBuilder("postgres:17.7")
            .WithDatabase("product_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithNetwork(network)
            .WithNetworkAliases("postgres-product")
            .Build();

        _rabbitMq = new RabbitMqBuilder("rabbitmq:3-management")
            .WithNetwork(network)
            .WithNetworkAliases("rabbitmq")
            .WithUsername("user")
            .WithPassword("password")
            .Build();
        
        _inventorySvc = new ContainerBuilder("one-rail/inventory-service")
            .WithPortBinding(7500, 80)
            .WithEnvironment("ConnectionStrings__Postgres",
                "Host=postgres-inventory;Port=5432;Database=inventory_db;Username=postgres;Password=postgres")
            .WithEnvironment("RabbitMQ__Host", "rabbitmq")
            .WithEnvironment("RabbitMQ__Username", "user")
            .WithEnvironment("RabbitMQ__Password", "password")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT","Development")
            .WithNetwork(network)
            .Build();
        
        _productSvc = new ContainerBuilder("one-rail/product-service")
            .WithPortBinding(7600, 80)
            .WithEnvironment("ConnectionStrings__Postgres",
                "Host=postgres-product;Port=5432;Database=product_db;Username=postgres;Password=postgres")
            .WithEnvironment("RabbitMQ__Host", "rabbitmq")
            .WithEnvironment("RabbitMQ__Username", "user")
            .WithEnvironment("RabbitMQ__Password", "password")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT","Development")
            .WithNetwork(network)
            .Build();
        
        await _postgresInventory.StartAsync();
        await _postgresProduct.StartAsync();
        await _rabbitMq.StartAsync();
        await _inventorySvc.StartAsync();
        await _productSvc.StartAsync();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add(
            "Authorization",
            [$"Bearer {TestAuthWrapper.GetToken()}"]);
    }
    
    [TestMethod]
    public async Task WholeScenario()
    {
        // Arrange
        var productResponse = await _client.PostAsJsonAsync(
            "http://localhost:7600/product/",
            new ProductCreateReq("Name", "Desc", 20.0m));
        productResponse.EnsureSuccessStatusCode();
        var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();
        
        // Act
        var inventoryResponse = await _client.PostAsJsonAsync(
            "http://localhost:7500/inventory/",
            new InventoryCreateReq(product!.Id, 20));
        inventoryResponse.EnsureSuccessStatusCode();
        
        // Assert
        var listResponse = await _client.GetAsync(
            "http://localhost:7600/product/");
        listResponse.EnsureSuccessStatusCode();
        var list = await listResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
        var item = list!.First(x => x.Id == product.Id);
        Assert.IsNotNull(item);
        Assert.AreEqual(20, item.Amount);
    }

    [TestCleanup]
    public async Task CleanupAsync()
    {
        await _postgresInventory.StopAsync();
        await _postgresProduct.StopAsync();
        await _rabbitMq.StopAsync();
        await _inventorySvc.StopAsync();
        await _productSvc.StopAsync();
    }
}