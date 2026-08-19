using Inventory.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product.ToTable("products");

        product.HasKey(p => p.Id);

        product.Property(p => p.Code)
            .HasMaxLength(50)
            .IsRequired();

        product.HasIndex(p => p.Code)
            .IsUnique();

        product.Property(p => p.Description)
            .HasMaxLength(200)
            .IsRequired();
        
        product.Property(p => p.Balance)
            .IsRequired();

        product.Property(p => p.CreatedAt)
            .IsRequired();
        
        product.ToTable(table => table.HasCheckConstraint("CK_product_balance_non_negative", "\"Balance\" >= 0"));
    }
}