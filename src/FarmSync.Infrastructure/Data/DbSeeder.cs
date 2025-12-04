using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Entities.ReferenceData;
using FarmSync.Domain.Entities.Procurement;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(FarmSyncDbContext context)
    {
        // Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new[]
            {
                new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "System Administrator", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Manager", Description = "Farm Manager", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "User", Description = "Standard User", IsActive = true }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true
            };
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            await context.UserRoles.AddAsync(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });
            await context.SaveChangesAsync();
        }

        // Seed Reference Data - Inventory Categories
        if (!await context.InventoryCategories.AnyAsync())
        {
            var categories = new[]
            {
                new InventoryCategory { Id = Guid.NewGuid(), Name = "Seeds", Description = "Agricultural seeds", IsActive = true },
                new InventoryCategory { Id = Guid.NewGuid(), Name = "Fertilizers", Description = "Soil fertilizers and nutrients", IsActive = true },
                new InventoryCategory { Id = Guid.NewGuid(), Name = "Pesticides", Description = "Pest control products", IsActive = true },
                new InventoryCategory { Id = Guid.NewGuid(), Name = "Tools", Description = "Hand tools and equipment", IsActive = true },
                new InventoryCategory { Id = Guid.NewGuid(), Name = "Supplies", Description = "General farm supplies", IsActive = true }
            };
            await context.InventoryCategories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // Seed Inventory Types
        if (!await context.InventoryTypes.AnyAsync())
        {
            var types = new[]
            {
                new InventoryType { Id = Guid.NewGuid(), Name = "Consumable", Description = "Items that are consumed", IsActive = true },
                new InventoryType { Id = Guid.NewGuid(), Name = "Durable", Description = "Long-lasting items", IsActive = true },
                new InventoryType { Id = Guid.NewGuid(), Name = "Perishable", Description = "Items with expiry dates", IsActive = true }
            };
            await context.InventoryTypes.AddRangeAsync(types);
            await context.SaveChangesAsync();
        }

        // Seed Units of Measure
        if (!await context.UnitsOfMeasure.AnyAsync())
        {
            var units = new[]
            {
                new UnitOfMeasure { Id = Guid.NewGuid(), Name = "Kilogram", Abbreviation = "kg", IsActive = true },
                new UnitOfMeasure { Id = Guid.NewGuid(), Name = "Liter", Abbreviation = "L", IsActive = true },
                new UnitOfMeasure { Id = Guid.NewGuid(), Name = "Piece", Abbreviation = "pc", IsActive = true },
                new UnitOfMeasure { Id = Guid.NewGuid(), Name = "Bag", Abbreviation = "bag", IsActive = true },
                new UnitOfMeasure { Id = Guid.NewGuid(), Name = "Box", Abbreviation = "box", IsActive = true }
            };
            await context.UnitsOfMeasure.AddRangeAsync(units);
            await context.SaveChangesAsync();
        }

        // Seed Transaction Statuses
        if (!await context.TransactionStatuses.AnyAsync())
        {
            var statuses = new[]
            {
                new TransactionStatus { Id = Guid.NewGuid(), Name = "Pending", Description = "Awaiting approval", IsActive = true },
                new TransactionStatus { Id = Guid.NewGuid(), Name = "Approved", Description = "Approved transaction", IsActive = true },
                new TransactionStatus { Id = Guid.NewGuid(), Name = "Completed", Description = "Completed transaction", IsActive = true },
                new TransactionStatus { Id = Guid.NewGuid(), Name = "Cancelled", Description = "Cancelled transaction", IsActive = true }
            };
            await context.TransactionStatuses.AddRangeAsync(statuses);
            await context.SaveChangesAsync();
        }

        // Seed Equipment Conditions
        if (!await context.EquipmentConditions.AnyAsync())
        {
            var conditions = new[]
            {
                new EquipmentCondition { Id = Guid.NewGuid(), Name = "Excellent", Description = "Like new condition", IsActive = true },
                new EquipmentCondition { Id = Guid.NewGuid(), Name = "Good", Description = "Working well", IsActive = true },
                new EquipmentCondition { Id = Guid.NewGuid(), Name = "Fair", Description = "Needs minor repairs", IsActive = true },
                new EquipmentCondition { Id = Guid.NewGuid(), Name = "Poor", Description = "Needs major repairs", IsActive = true }
            };
            await context.EquipmentConditions.AddRangeAsync(conditions);
            await context.SaveChangesAsync();
        }

        // Seed Maintenance Types
        if (!await context.MaintenanceTypes.AnyAsync())
        {
            var maintenanceTypes = new[]
            {
                new MaintenanceType { Id = Guid.NewGuid(), Name = "Routine Maintenance", Description = "Regular scheduled maintenance", IsActive = true },
                new MaintenanceType { Id = Guid.NewGuid(), Name = "Repair", Description = "Fix broken or damaged equipment", IsActive = true },
                new MaintenanceType { Id = Guid.NewGuid(), Name = "Inspection", Description = "Safety and condition inspection", IsActive = true },
                new MaintenanceType { Id = Guid.NewGuid(), Name = "Upgrade", Description = "Equipment upgrade or modification", IsActive = true }
            };
            await context.MaintenanceTypes.AddRangeAsync(maintenanceTypes);
            await context.SaveChangesAsync();
        }

        // Seed Inventory Locations
        if (!await context.InventoryLocations.AnyAsync())
        {
            var locations = new[]
            {
                new InventoryLocation { Id = Guid.NewGuid(), Name = "Main Warehouse", Description = "Central storage facility", Address = "Farm Location A", IsActive = true },
                new InventoryLocation { Id = Guid.NewGuid(), Name = "Field Storage", Description = "On-site field storage", Address = "Field Section B", IsActive = true },
                new InventoryLocation { Id = Guid.NewGuid(), Name = "Equipment Shed", Description = "Equipment and tools storage", Address = "Workshop Area", IsActive = true }
            };
            await context.InventoryLocations.AddRangeAsync(locations);
            await context.SaveChangesAsync();
        }

        // Seed Suppliers
        if (!await context.Suppliers.AnyAsync())
        {
            var suppliers = new[]
            {
                new Supplier 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "AgriSupply Co.", 
                    ContactPerson = "John Smith",
                    Email = "sales@agrisupply.com",
                    Phone = "+27 11 123 4567",
                    Address = "123 Industrial Road, Johannesburg",
                    TaxNumber = "9876543210",
                    IsActive = true 
                },
                new Supplier 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "FarmTech Solutions", 
                    ContactPerson = "Sarah Johnson",
                    Email = "info@farmtech.co.za",
                    Phone = "+27 21 987 6543",
                    Address = "456 Commerce Street, Cape Town",
                    TaxNumber = "1234567890",
                    IsActive = true 
                },
                new Supplier 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Green Valley Supplies", 
                    ContactPerson = "Michael Brown",
                    Email = "contact@greenvalley.co.za",
                    Phone = "+27 31 555 7890",
                    Address = "789 Agricultural Avenue, Durban",
                    TaxNumber = "5555666677",
                    IsActive = true 
                }
            };
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }
    }
}
