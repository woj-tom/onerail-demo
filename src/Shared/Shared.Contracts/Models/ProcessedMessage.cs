namespace Shared.Contracts.Models;

public class ProcessedMessage
{
    public Guid MessageId { get; set; }
    public string MessageType { get; set; }
    public DateTime ProcessedAt { get; set; }
    public DateTime OccurredAt { get; set; }
}