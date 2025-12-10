using FarmSync.Domain.Entities.Auth;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Entities.ReferenceData;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Entities.Fleet;
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
                new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "System Administrator with full access", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Operations Manager", Description = "Oversees farm operations, approves high-level transactions", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Accounting Manager", Description = "Financial oversight, approves purchase orders", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Operations Clerk", Description = "Receives goods, handles clock-ins, issues inventory", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Accountant", Description = "View-only financial reports", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Team Leader", Description = "Manages team, approves inventory for team", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "Driver", Description = "Fleet operations, refueling approval", IsActive = true },
                new Role { Id = Guid.NewGuid(), Name = "General Worker", Description = "Standard employee access", IsActive = true }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed Test Users for Each Role
        if (!await context.Users.AnyAsync())
        {
            var roles = await context.Roles.ToListAsync();
            var users = new List<User>();
            var userRoles = new List<UserRole>();

            // Admin User
            var adminRole = roles.First(r => r.Name == "Admin");
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
            users.Add(adminUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = adminUser.Id, RoleId = adminRole.Id });

            // Operations Manager
            var opsManagerRole = roles.First(r => r.Name == "Operations Manager");
            var opsManagerUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "opsmanager",
                Email = "operations.manager@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                FirstName = "John",
                LastName = "Ndlovu",
                IsActive = true
            };
            users.Add(opsManagerUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = opsManagerUser.Id, RoleId = opsManagerRole.Id });

            // Accounting Manager
            var accManagerRole = roles.First(r => r.Name == "Accounting Manager");
            var accManagerUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "accmanager",
                Email = "accounting.manager@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                FirstName = "Sarah",
                LastName = "Dlamini",
                IsActive = true
            };
            users.Add(accManagerUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = accManagerUser.Id, RoleId = accManagerRole.Id });

            // Operations Clerk
            var opsClerkRole = roles.First(r => r.Name == "Operations Clerk");
            var opsClerkUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "opsclerk",
                Email = "operations.clerk@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Clerk@123"),
                FirstName = "Thabo",
                LastName = "Mthembu",
                IsActive = true
            };
            users.Add(opsClerkUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = opsClerkUser.Id, RoleId = opsClerkRole.Id });

            // Accountant
            var accountantRole = roles.First(r => r.Name == "Accountant");
            var accountantUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "accountant",
                Email = "accountant@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Accountant@123"),
                FirstName = "Nomsa",
                LastName = "Khumalo",
                IsActive = true
            };
            users.Add(accountantUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = accountantUser.Id, RoleId = accountantRole.Id });

            // Team Leader
            var teamLeaderRole = roles.First(r => r.Name == "Team Leader");
            var teamLeaderUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "teamleader",
                Email = "team.leader@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Leader@123"),
                FirstName = "Sipho",
                LastName = "Zulu",
                IsActive = true
            };
            users.Add(teamLeaderUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = teamLeaderUser.Id, RoleId = teamLeaderRole.Id });

            // Driver
            var driverRole = roles.First(r => r.Name == "Driver");
            var driverUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "driver",
                Email = "driver@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Driver@123"),
                FirstName = "Lucky",
                LastName = "Mokoena",
                IsActive = true
            };
            users.Add(driverUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = driverUser.Id, RoleId = driverRole.Id });

            // General Worker
            var workerRole = roles.First(r => r.Name == "General Worker");
            var workerUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "worker",
                Email = "worker@farmsync.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Worker@123"),
                FirstName = "Mandla",
                LastName = "Nkosi",
                IsActive = true
            };
            users.Add(workerUser);
            userRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = workerUser.Id, RoleId = workerRole.Id });

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            await context.UserRoles.AddRangeAsync(userRoles);
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
                new Domain.Entities.ReferenceData.MaintenanceType { Id = Guid.NewGuid(), Name = "Routine Maintenance", Description = "Regular scheduled maintenance", IsActive = true },
                new Domain.Entities.ReferenceData.MaintenanceType { Id = Guid.NewGuid(), Name = "Repair", Description = "Fix broken or damaged equipment", IsActive = true },
                new Domain.Entities.ReferenceData.MaintenanceType { Id = Guid.NewGuid(), Name = "Inspection", Description = "Safety and condition inspection", IsActive = true },
                new Domain.Entities.ReferenceData.MaintenanceType { Id = Guid.NewGuid(), Name = "Upgrade", Description = "Equipment upgrade or modification", IsActive = true }
            };
            await context.MaintenanceTypes.AddRangeAsync(maintenanceTypes);
            await context.SaveChangesAsync();
        }

        // Seed Document Types
        if (!await context.DocumentTypes.AnyAsync())
        {
            var documentTypes = new[]
            {
                new DocumentType { Id = Guid.NewGuid(), Name = "Quotation", Description = "Price quotation from supplier", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Invoice", Description = "Payment invoice", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Proof of Payment (POP)", Description = "Payment confirmation", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Delivery Note", Description = "Goods delivery confirmation", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Sick Note", Description = "Medical certificate", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Contract", Description = "Employment or service contract", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "ID Document", Description = "Identification document", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Driver's License", Description = "Driver's license document", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Service Report", Description = "Maintenance service report", IsActive = true },
                new DocumentType { Id = Guid.NewGuid(), Name = "Other", Description = "Other supporting documents", IsActive = true }
            };
            await context.DocumentTypes.AddRangeAsync(documentTypes);
            await context.SaveChangesAsync();
        }

        // Seed Leave Types
        if (!await context.LeaveTypes.AnyAsync())
        {
            var leaveTypes = new[]
            {
                new LeaveType { Id = Guid.NewGuid(), Name = "Annual Leave", Description = "Paid annual vacation leave", RequiresDocument = false, DefaultDaysPerYear = 15, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Sick Leave", Description = "Medical leave", RequiresDocument = true, DefaultDaysPerYear = 10, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Family Responsibility Leave", Description = "Leave for family matters", RequiresDocument = true, DefaultDaysPerYear = 3, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Unpaid Leave", Description = "Leave without pay", RequiresDocument = false, DefaultDaysPerYear = 0, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Study Leave", Description = "Educational purposes", RequiresDocument = true, DefaultDaysPerYear = 5, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Maternity Leave", Description = "Maternity leave", RequiresDocument = true, DefaultDaysPerYear = 90, IsActive = true },
                new LeaveType { Id = Guid.NewGuid(), Name = "Paternity Leave", Description = "Paternity leave", RequiresDocument = true, DefaultDaysPerYear = 10, IsActive = true }
            };
            await context.LeaveTypes.AddRangeAsync(leaveTypes);
            await context.SaveChangesAsync();
        }

        // Seed Field Phases
        if (!await context.FieldPhases.AnyAsync())
        {
            var fieldPhases = new[]
            {
                new FieldPhase { Id = Guid.NewGuid(), Name = "Land Preparation", Description = "Preparing soil for planting", SortOrder = 1, IsActive = true },
                new FieldPhase { Id = Guid.NewGuid(), Name = "Planting", Description = "Planting seeds or seedlings", SortOrder = 2, IsActive = true },
                new FieldPhase { Id = Guid.NewGuid(), Name = "Growing", Description = "Crop growth and maintenance", SortOrder = 3, IsActive = true },
                new FieldPhase { Id = Guid.NewGuid(), Name = "Harvesting", Description = "Harvesting mature crops", SortOrder = 4, IsActive = true },
                new FieldPhase { Id = Guid.NewGuid(), Name = "Haulage", Description = "Transporting harvested crops", SortOrder = 5, IsActive = true },
                new FieldPhase { Id = Guid.NewGuid(), Name = "Fallow", Description = "Resting period between crops", SortOrder = 6, IsActive = true }
            };
            await context.FieldPhases.AddRangeAsync(fieldPhases);
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

        // Seed HR - Positions
        if (!await context.Positions.AnyAsync())
        {
            var positions = new[]
            {
                new Position { Id = Guid.NewGuid(), Name = "Farm Worker", Rate = 150.00m },
                new Position { Id = Guid.NewGuid(), Name = "Tractor Driver", Rate = 200.00m },
                new Position { Id = Guid.NewGuid(), Name = "Irrigation Specialist", Rate = 180.00m },
                new Position { Id = Guid.NewGuid(), Name = "Harvesting Supervisor", Rate = 250.00m },
                new Position { Id = Guid.NewGuid(), Name = "Team Leader", Rate = 220.00m },
                new Position { Id = Guid.NewGuid(), Name = "Packing Supervisor", Rate = 210.00m },
                new Position { Id = Guid.NewGuid(), Name = "Spraying Operator", Rate = 190.00m },
                new Position { Id = Guid.NewGuid(), Name = "Maintenance Technician", Rate = 230.00m },
                new Position { Id = Guid.NewGuid(), Name = "Store Clerk", Rate = 170.00m }
            };
            await context.Positions.AddRangeAsync(positions);
            await context.SaveChangesAsync();
        }

        // Seed HR - Employment Types
        if (!await context.EmploymentTypes.AnyAsync())
        {
            var employmentTypes = new[]
            {
                new EmploymentType { Id = Guid.NewGuid(), Name = "Permanent" },
                new EmploymentType { Id = Guid.NewGuid(), Name = "Seasonal" },
                new EmploymentType { Id = Guid.NewGuid(), Name = "Contractor" },
                new EmploymentType { Id = Guid.NewGuid(), Name = "Temporary" }
            };
            await context.EmploymentTypes.AddRangeAsync(employmentTypes);
            await context.SaveChangesAsync();
        }

        // Seed HR - Team Types
        if (!await context.TeamTypes.AnyAsync())
        {
            var teamTypes = new[]
            {
                new TeamType { Id = Guid.NewGuid(), Name = "Harvesting" },
                new TeamType { Id = Guid.NewGuid(), Name = "Spraying" },
                new TeamType { Id = Guid.NewGuid(), Name = "Packing" },
                new TeamType { Id = Guid.NewGuid(), Name = "Irrigation" },
                new TeamType { Id = Guid.NewGuid(), Name = "Maintenance" },
                new TeamType { Id = Guid.NewGuid(), Name = "Planting" },
                new TeamType { Id = Guid.NewGuid(), Name = "Pruning" }
            };
            await context.TeamTypes.AddRangeAsync(teamTypes);
            await context.SaveChangesAsync();
        }

        // Seed HR - Work Areas
        if (!await context.WorkAreas.AnyAsync())
        {
            var workAreas = new[]
            {
                new WorkArea { Id = Guid.NewGuid(), Name = "Field A - North", SizeUnit = "Hectares", Size = 25.5m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Field B - East", SizeUnit = "Hectares", Size = 30.0m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Field C - South", SizeUnit = "Hectares", Size = 18.75m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Field D - West", SizeUnit = "Hectares", Size = 22.3m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Greenhouse 1", SizeUnit = "Square Meters", Size = 500.0m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Greenhouse 2", SizeUnit = "Square Meters", Size = 450.0m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Orchard Section", SizeUnit = "Hectares", Size = 12.0m },
                new WorkArea { Id = Guid.NewGuid(), Name = "Nursery", SizeUnit = "Square Meters", Size = 200.0m }
            };
            await context.WorkAreas.AddRangeAsync(workAreas);
            await context.SaveChangesAsync();
        }

        // Seed HR - Bank Names
        if (!await context.BankNames.AnyAsync())
        {
            var banks = new[]
            {
                new BankName { Id = Guid.NewGuid(), Name = "ABSA Bank" },
                new BankName { Id = Guid.NewGuid(), Name = "Standard Bank" },
                new BankName { Id = Guid.NewGuid(), Name = "FNB" },
                new BankName { Id = Guid.NewGuid(), Name = "Nedbank" },
                new BankName { Id = Guid.NewGuid(), Name = "Capitec Bank" }
            };
            await context.BankNames.AddRangeAsync(banks);
            await context.SaveChangesAsync();
        }

        // Seed HR - Account Types
        if (!await context.AccountTypes.AnyAsync())
        {
            var accountTypes = new[]
            {
                new AccountType { Id = Guid.NewGuid(), Name = "Savings" },
                new AccountType { Id = Guid.NewGuid(), Name = "Cheque" },
                new AccountType { Id = Guid.NewGuid(), Name = "Current" }
            };
            await context.AccountTypes.AddRangeAsync(accountTypes);
            await context.SaveChangesAsync();
        }

        // Seed HR - Task Statuses
        if (!await context.TaskStatuses.AnyAsync())
        {
            var taskStatuses = new[]
            {
                new Domain.Entities.HR.TaskStatus { Id = Guid.NewGuid(), Name = "Pending" },
                new Domain.Entities.HR.TaskStatus { Id = Guid.NewGuid(), Name = "InProgress" },
                new Domain.Entities.HR.TaskStatus { Id = Guid.NewGuid(), Name = "Completed" },
                new Domain.Entities.HR.TaskStatus { Id = Guid.NewGuid(), Name = "Cancelled" }
            };
            await context.TaskStatuses.AddRangeAsync(taskStatuses);
            await context.SaveChangesAsync();
        }

        // Seed HR - Issue Statuses
        if (!await context.IssueStatuses.AnyAsync())
        {
            var issueStatuses = new[]
            {
                new IssueStatus { Id = Guid.NewGuid(), Name = "Pending" },
                new IssueStatus { Id = Guid.NewGuid(), Name = "Approved" },
                new IssueStatus { Id = Guid.NewGuid(), Name = "Issued" },
                new IssueStatus { Id = Guid.NewGuid(), Name = "Returned" },
                new IssueStatus { Id = Guid.NewGuid(), Name = "Cancelled" }
            };
            await context.IssueStatuses.AddRangeAsync(issueStatuses);
            await context.SaveChangesAsync();
        }

        // Seed Fleet - Vehicle Types
        if (!await context.VehicleTypes.AnyAsync())
        {
            var vehicleTypes = new[]
            {
                new VehicleType { Id = Guid.NewGuid(), Name = "Tractor", Description = "Agricultural tractor", IsActive = true },
                new VehicleType { Id = Guid.NewGuid(), Name = "Truck", Description = "Farm truck for transport", IsActive = true },
                new VehicleType { Id = Guid.NewGuid(), Name = "Van", Description = "Utility van", IsActive = true },
                new VehicleType { Id = Guid.NewGuid(), Name = "Trailer", Description = "Agricultural trailer", IsActive = true },
                new VehicleType { Id = Guid.NewGuid(), Name = "Utility Vehicle", Description = "Multi-purpose utility vehicle", IsActive = true },
                new VehicleType { Id = Guid.NewGuid(), Name = "ATV", Description = "All-terrain vehicle", IsActive = true }
            };
            await context.VehicleTypes.AddRangeAsync(vehicleTypes);
            await context.SaveChangesAsync();
        }

        // Seed Fleet - Vehicle Statuses
        if (!await context.VehicleStatuses.AnyAsync())
        {
            var vehicleStatuses = new[]
            {
                new VehicleStatus { Id = Guid.NewGuid(), Name = "Active", Description = "Vehicle is operational and available", IsActive = true },
                new VehicleStatus { Id = Guid.NewGuid(), Name = "In Maintenance", Description = "Vehicle is under maintenance", IsActive = true },
                new VehicleStatus { Id = Guid.NewGuid(), Name = "In Repair", Description = "Vehicle is being repaired", IsActive = true },
                new VehicleStatus { Id = Guid.NewGuid(), Name = "Decommissioned", Description = "Vehicle is no longer in service", IsActive = true },
                new VehicleStatus { Id = Guid.NewGuid(), Name = "Reserved", Description = "Vehicle is reserved for specific use", IsActive = true }
            };
            await context.VehicleStatuses.AddRangeAsync(vehicleStatuses);
            await context.SaveChangesAsync();
        }

        // Seed Fleet - Fuel Types
        if (!await context.FuelTypes.AnyAsync())
        {
            var fuelTypes = new[]
            {
                new FuelType { Id = Guid.NewGuid(), Name = "Diesel", Description = "Diesel fuel", IsActive = true },
                new FuelType { Id = Guid.NewGuid(), Name = "Petrol", Description = "Gasoline/Petrol", IsActive = true },
                new FuelType { Id = Guid.NewGuid(), Name = "Electric", Description = "Electric battery power", IsActive = true },
                new FuelType { Id = Guid.NewGuid(), Name = "Hybrid", Description = "Hybrid fuel system", IsActive = true },
                new FuelType { Id = Guid.NewGuid(), Name = "LPG", Description = "Liquefied petroleum gas", IsActive = true }
            };
            await context.FuelTypes.AddRangeAsync(fuelTypes);
            await context.SaveChangesAsync();
        }

        // Seed Fleet - Maintenance Types
        if (!await context.FleetMaintenanceTypes.AnyAsync())
        {
            var fleetMaintenanceTypes = new[]
            {
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Oil Change", Description = "Engine oil and filter change", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Tire Rotation", Description = "Tire rotation and balance", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Brake Service", Description = "Brake inspection and service", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Engine Service", Description = "Engine tune-up and service", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Electrical", Description = "Electrical system maintenance", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Body Work", Description = "Body repair and painting", IsActive = true },
                new Domain.Entities.Fleet.MaintenanceType { Id = Guid.NewGuid(), Name = "Inspection", Description = "Safety and condition inspection", IsActive = true }
            };
            await context.FleetMaintenanceTypes.AddRangeAsync(fleetMaintenanceTypes);
            await context.SaveChangesAsync();
        }

        // Seed Fleet - Transport Task Statuses
        if (!await context.FleetTaskStatuses.AnyAsync())
        {
            var taskStatuses = new[]
            {
                new Domain.Entities.Fleet.TaskStatus { Id = Guid.NewGuid(), Name = "Pending", Description = "Task is pending", IsActive = true },
                new Domain.Entities.Fleet.TaskStatus { Id = Guid.NewGuid(), Name = "In Progress", Description = "Task is in progress", IsActive = true },
                new Domain.Entities.Fleet.TaskStatus { Id = Guid.NewGuid(), Name = "Completed", Description = "Task is completed", IsActive = true },
                new Domain.Entities.Fleet.TaskStatus { Id = Guid.NewGuid(), Name = "Cancelled", Description = "Task was cancelled", IsActive = true },
                new Domain.Entities.Fleet.TaskStatus { Id = Guid.NewGuid(), Name = "On Hold", Description = "Task is on hold", IsActive = true }
            };
            await context.FleetTaskStatuses.AddRangeAsync(taskStatuses);
            await context.SaveChangesAsync();
        }
    }
}
