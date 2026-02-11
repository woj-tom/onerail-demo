using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>(entity =>
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
        });
    }
}