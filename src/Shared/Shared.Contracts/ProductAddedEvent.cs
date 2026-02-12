namespace Shared.Contracts;

public record ProductAddedEvent(
    Guid EventId,
    Guid ProductId,
    string ProductName,
    DateTime OccurredAt);