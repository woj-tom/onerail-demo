using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Models;
using Shared.Contracts.Repositories;

namespace ProductService.Infrastructure.Database.Repositories;

public class ProcessedMessageRepository(AppDbContext db) : IProcessedMessageRepository
{
    public async Task CreateAsync(ProcessedMessage message, CancellationToken ct)
    {
        await db.ProcessedMessages.AddAsync(message, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await db.ProcessedMessages.AnyAsync(message => message.MessageId == id, ct);
    }
}