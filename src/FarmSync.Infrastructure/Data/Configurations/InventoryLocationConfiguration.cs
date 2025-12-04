using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class InventoryLocationConfiguration : IEntityTypeConfiguration<InventoryLocation>
{
    public void Configure(EntityTypeBuilder<InventoryLocation> builder)
    {
        builder.ToTable("InventoryLocations");
        
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(l => l.Description)
            .HasMaxLength(500);
        
        builder.Property(l => l.Address)
            .HasMaxLength(200);
        
        builder.HasIndex(l => l.Name)
            .IsUnique();
    }
}
