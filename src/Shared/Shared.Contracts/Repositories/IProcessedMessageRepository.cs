using Shared.Contracts.Models;

namespace Shared.Contracts.Repositories;

public interface IProcessedMessageRepository
{
    public Task InsertAsync(ProcessedMessage message, CancellationToken ct);
    
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}