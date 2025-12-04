using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class EquipmentMaintenanceRecordConfiguration : IEntityTypeConfiguration<EquipmentMaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<EquipmentMaintenanceRecord> builder)
    {
        builder.ToTable("EquipmentMaintenanceRecords");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Description)
            .HasMaxLength(500);
        
        builder.Property(m => m.Cost)
            .HasPrecision(18, 2);
        
        builder.Property(m => m.PerformedBy)
            .HasMaxLength(100);
        
        builder.Property(m => m.Notes)
            .HasMaxLength(1000);
        
        builder.HasOne(m => m.Equipment)
            .WithMany(e => e.MaintenanceRecords)
            .HasForeignKey(m => m.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(m => m.MaintenanceType)
            .WithMany()
            .HasForeignKey(m => m.MaintenanceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(m => m.MaintenanceDate);
    }
}
