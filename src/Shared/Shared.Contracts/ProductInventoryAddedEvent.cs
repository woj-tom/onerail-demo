namespace Shared.Contracts;

public sealed record ProductInventoryAddedEvent(
    Guid EventId,
    Guid ProductId,
    int Quantity,
    DateTime OccurredAt);
