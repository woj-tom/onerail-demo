using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<InventoryEntry> Inventories => Set<InventoryEntry>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryEntry>(entity =>
        {
            entity.ToTable("inventory");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            entity.Property(e => e.AddedAt)
                .HasColumnName("added_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            entity.Property(e => e.AddedBy)
                .HasColumnName("added_by")
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(e => e.ProductId);
        });
    }
}