namespace Shared.Contracts;

public record ProductInventoryAddedEvent {
    public Guid EventId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public DateTime OccurredAt { get; init; }      
}