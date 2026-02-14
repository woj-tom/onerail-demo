namespace ProductService.Domain.Entities;

public class Product
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public int Amount { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public Product(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be null or empty", nameof(name));
        
        if (price <= 0)
            throw new ArgumentException("Product price cannot be zero or negative", nameof(price));

        Name = name;
        Description = description;
        Price = price;
    }
    
    public void IncreaseStock(int amount)
    {
        Amount += amount;
        UpdatedAt = DateTime.UtcNow;
    }
}