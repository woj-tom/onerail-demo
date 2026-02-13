using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Database.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
            entity.ToTable("product");

            entity.HasKey(product => product.Id);

            entity.Property(product => product.Id)
                .HasColumnName("id");

            entity.Property(product => product.Name)
                .HasColumnName("name")
                .IsRequired();

            entity.Property(product => product.Description)
                .HasColumnName("description")
                .IsRequired();
            
            entity.Property(product => product.Price)
                .HasColumnName("price")
                .IsRequired();
            
            entity.Property(product => product.Amount)
                .HasColumnName("amount")
                .IsRequired();

            entity.Property(product => product.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            entity.Property(product => product.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();
    }
}