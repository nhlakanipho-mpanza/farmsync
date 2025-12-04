using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Entities.ReferenceData;
using FarmSync.Domain.Entities.Procurement;

namespace FarmSync.Infrastructure.Data;

public class FarmSyncDbContext : DbContext
{
    public FarmSyncDbContext(DbContextOptions<FarmSyncDbContext> options) : base(options)
    {
    }

    // Auth
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    // Inventory
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryLocation> InventoryLocations => Set<InventoryLocation>();
    public DbSet<StockLevel> StockLevels => Set<StockLevel>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<EquipmentMaintenanceRecord> EquipmentMaintenanceRecords => Set<EquipmentMaintenanceRecord>();

    // Procurement
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<GoodsReceived> GoodsReceived => Set<GoodsReceived>();
    public DbSet<GoodsReceivedItem> GoodsReceivedItems => Set<GoodsReceivedItem>();

    // Reference Data
    public DbSet<InventoryCategory> InventoryCategories => Set<InventoryCategory>();
    public DbSet<InventoryType> InventoryTypes => Set<InventoryType>();
    public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
    public DbSet<TransactionStatus> TransactionStatuses => Set<TransactionStatus>();
    public DbSet<EquipmentCondition> EquipmentConditions => Set<EquipmentCondition>();
    public DbSet<MaintenanceType> MaintenanceTypes => Set<MaintenanceType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmSyncDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is Domain.Common.BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow.AddHours(2);
                }
                baseEntity.UpdatedAt = DateTime.UtcNow.AddHours(2);
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
