namespace InventoryService.Domain.Entities;

public class RegisteredProduct
{
    public Guid Id { get; }
    public string Name { get; }
    public DateTime AddedAt { get; } = DateTime.UtcNow;
    
    public RegisteredProduct(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}