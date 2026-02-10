using InventoryService.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Inventory> Inventories => Set<Inventory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>(entity =>
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