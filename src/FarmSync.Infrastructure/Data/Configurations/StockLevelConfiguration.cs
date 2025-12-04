using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
{
    public void Configure(EntityTypeBuilder<StockLevel> builder)
    {
        builder.ToTable("StockLevels");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Quantity)
            .HasPrecision(18, 2);
        
        builder.Property(s => s.ReservedQuantity)
            .HasPrecision(18, 2);
        
        builder.HasOne(s => s.InventoryItem)
            .WithMany(i => i.StockLevels)
            .HasForeignKey(s => s.InventoryItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(s => s.Location)
            .WithMany(l => l.StockLevels)
            .HasForeignKey(s => s.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(s => new { s.InventoryItemId, s.LocationId })
            .IsUnique();
        
        builder.Ignore(s => s.AvailableQuantity);
    }
}
