using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryService.Infrastructure.Database.Configurations;

public class RegisteredProductConfiguration : IEntityTypeConfiguration<RegisteredProduct>
{
    public void Configure(EntityTypeBuilder<RegisteredProduct> entity)
    {
        entity.ToTable("registered_product");
            
        entity.HasKey(product => product.Id);

        entity.Property(product => product.Id)
            .HasColumnName("id");
            
        entity.Property(product => product.Name)
            .HasColumnName("name")
            .IsRequired();
            
        entity.Property(product => product.AddedAt)
            .HasColumnName("added_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
    }
}