using Shared.Contracts.Models;

namespace Shared.Contracts.Repositories;

public interface IProcessedMessageRepository
{
    public Task CreateAsync(ProcessedMessage message, CancellationToken ct);
    
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}