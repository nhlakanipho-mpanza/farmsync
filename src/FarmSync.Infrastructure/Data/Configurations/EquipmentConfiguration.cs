using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipment");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Description)
            .HasMaxLength(500);
        
        builder.Property(e => e.SerialNumber)
            .HasMaxLength(50);
        
        builder.Property(e => e.Model)
            .HasMaxLength(100);
        
        builder.Property(e => e.Manufacturer)
            .HasMaxLength(100);
        
        builder.Property(e => e.PurchasePrice)
            .HasPrecision(18, 2);
        
        builder.HasOne(e => e.Condition)
            .WithMany()
            .HasForeignKey(e => e.ConditionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(e => e.SerialNumber)
            .IsUnique()
            .HasFilter("\"SerialNumber\" IS NOT NULL");
    }
}
