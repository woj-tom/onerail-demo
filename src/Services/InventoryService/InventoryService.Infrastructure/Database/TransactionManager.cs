using Microsoft.EntityFrameworkCore.Storage;
using Shared.Contracts;

namespace InventoryService.Infrastructure.Database;

public class TransactionManager(AppDbContext db) : ITransactionManager
{
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return db.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }
}