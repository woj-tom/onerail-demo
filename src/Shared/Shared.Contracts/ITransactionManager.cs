using Microsoft.EntityFrameworkCore.Storage;

namespace Shared.Contracts;

public interface ITransactionManager
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}