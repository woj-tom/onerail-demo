using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Database.Configurations;

public class InventoryEntryConfiguration : IEntityTypeConfiguration<InventoryEntry>
{
    public void Configure(EntityTypeBuilder<InventoryEntry> entity)
    {
        entity.ToTable("inventory_entry");

        entity.HasKey(entry => entry.Id);

        entity.Property(entry => entry.Id)
            .HasColumnName("id");

        entity.Property(entry => entry.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        entity.Property(entry => entry.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        entity.Property(entry => entry.AddedAt)
            .HasColumnName("added_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        entity.Property(entry => entry.AddedBy)
            .HasColumnName("added_by")
            .HasMaxLength(200)
            .IsRequired();

        entity.HasIndex(entry => entry.ProductId);
    }
}