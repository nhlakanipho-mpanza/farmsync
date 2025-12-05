using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Entities.ReferenceData;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Entities.Fleet;

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
    public DbSet<FarmSync.Domain.Entities.ReferenceData.MaintenanceType> MaintenanceTypes => Set<FarmSync.Domain.Entities.ReferenceData.MaintenanceType>();

    // HR - Employees
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    public DbSet<BankDetails> BankDetails => Set<BankDetails>();
    public DbSet<BiometricEnrolment> BiometricEnrolments => Set<BiometricEnrolment>();

    // HR - Teams & Tasks
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<WorkArea> WorkAreas => Set<WorkArea>();
    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();
    public DbSet<ClockEvent> ClockEvents => Set<ClockEvent>();

    // HR - Issuing
    public DbSet<InventoryIssue> InventoryIssues => Set<InventoryIssue>();
    public DbSet<EquipmentIssue> EquipmentIssues => Set<EquipmentIssue>();

    // HR - Reference Data
    public DbSet<EmploymentType> EmploymentTypes => Set<EmploymentType>();
    public DbSet<RoleType> RoleTypes => Set<RoleType>();
    public DbSet<TeamType> TeamTypes => Set<TeamType>();
    public DbSet<BankName> BankNames => Set<BankName>();
    public DbSet<AccountType> AccountTypes => Set<AccountType>();
    public DbSet<Domain.Entities.HR.TaskStatus> TaskStatuses => Set<Domain.Entities.HR.TaskStatus>();
    public DbSet<IssueStatus> IssueStatuses => Set<IssueStatus>();

    // Fleet - Vehicles
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<VehicleStatus> VehicleStatuses => Set<VehicleStatus>();
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<DriverAssignment> DriverAssignments => Set<DriverAssignment>();

    // Fleet - Operations
    public DbSet<TripLog> TripLogs => Set<TripLog>();
    public DbSet<GPSLocation> GPSLocations => Set<GPSLocation>();
    public DbSet<TransportTask> TransportTasks => Set<TransportTask>();
    public DbSet<Domain.Entities.Fleet.TaskStatus> FleetTaskStatuses => Set<Domain.Entities.Fleet.TaskStatus>();
    public DbSet<Geofence> Geofences => Set<Geofence>();
    public DbSet<SpeedingEvent> SpeedingEvents => Set<SpeedingEvent>();

    // Fleet - Maintenance & Fuel
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<Domain.Entities.Fleet.MaintenanceType> FleetMaintenanceTypes => Set<Domain.Entities.Fleet.MaintenanceType>();
    public DbSet<FuelLog> FuelLogs => Set<FuelLog>();
    public DbSet<IncidentReport> IncidentReports => Set<IncidentReport>();

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
