using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(i => i.Description)
            .HasMaxLength(500);
        
        builder.Property(i => i.SKU)
            .HasMaxLength(50);
        
        builder.Property(i => i.MinimumStockLevel)
            .HasPrecision(18, 2);
        
        builder.Property(i => i.ReorderLevel)
            .HasPrecision(18, 2);
        
        builder.Property(i => i.UnitPrice)
            .HasPrecision(18, 2);
        
        builder.HasOne(i => i.Category)
            .WithMany()
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(i => i.Type)
            .WithMany()
            .HasForeignKey(i => i.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(i => i.UnitOfMeasure)
            .WithMany()
            .HasForeignKey(i => i.UnitOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(i => i.SKU)
            .IsUnique()
            .HasFilter("\"SKU\" IS NOT NULL");
    }
}
