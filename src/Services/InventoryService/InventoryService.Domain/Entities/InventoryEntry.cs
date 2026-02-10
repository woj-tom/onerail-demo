namespace InventoryService.Domain.Entities;

public class InventoryEntry
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ProductId { get; }
    public int Quantity { get; }
    public DateTime AddedAt { get; } = DateTime.UtcNow;
    public string AddedBy { get; }

    public InventoryEntry(Guid productId, int quantity, string addedBy)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        ProductId = productId;
        Quantity = quantity;
        AddedBy = addedBy;
    }
}