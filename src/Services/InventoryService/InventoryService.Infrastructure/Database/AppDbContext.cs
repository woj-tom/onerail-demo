using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Models;

namespace InventoryService.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<InventoryEntry> InventoryEntries => Set<InventoryEntry>();
    public DbSet<RegisteredProduct> RegisteredProducts => Set<RegisteredProduct>();
    public DbSet<ProcessedMessage> ProcessedMessages => Set<ProcessedMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}