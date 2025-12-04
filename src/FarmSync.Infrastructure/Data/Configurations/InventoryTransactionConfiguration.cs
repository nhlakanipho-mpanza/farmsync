using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Infrastructure.Data.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("InventoryTransactions");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(t => t.Quantity)
            .HasPrecision(18, 2);
        
        builder.Property(t => t.UnitCost)
            .HasPrecision(18, 2);
        
        builder.Property(t => t.TotalCost)
            .HasPrecision(18, 2);
        
        builder.Property(t => t.ReferenceNumber)
            .HasMaxLength(50);
        
        builder.Property(t => t.Notes)
            .HasMaxLength(1000);
        
        builder.Property(t => t.ApprovedBy)
            .HasMaxLength(100);
        
        builder.HasOne(t => t.InventoryItem)
            .WithMany(i => i.Transactions)
            .HasForeignKey(t => t.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t => t.Location)
            .WithMany(l => l.Transactions)
            .HasForeignKey(t => t.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(t => t.Status)
            .WithMany()
            .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(t => t.ReferenceNumber);
        builder.HasIndex(t => t.TransactionDate);
    }
}
