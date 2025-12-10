using FarmSync.Domain.Entities.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.API.Data;

public static class GPSDataSeeder
{
    public static async Task SeedGPSData(FarmSyncDbContext context)
    {
        // Remove old seeded GPS data (data older than 1 hour or with source "GPS Seeder")
        var oldSeededData = context.GPSLocations
            .Where(g => g.Timestamp < DateTime.UtcNow.AddHours(-1))
            .ToList();
        
        if (oldSeededData.Any())
        {
            context.GPSLocations.RemoveRange(oldSeededData);
            await context.SaveChangesAsync();
            Console.WriteLine($"Removed {oldSeededData.Count} old GPS location records");
        }

        // Check if we already have recent GPS data (within last hour)
        if (context.GPSLocations.Any(g => g.Timestamp >= DateTime.UtcNow.AddHours(-1)))
        {
            Console.WriteLine("Recent GPS data already exists, skipping seed");
            return;
        }

        // Get first vehicle for testing (or create if none exists)
        var vehicle = context.Vehicles.FirstOrDefault();
        if (vehicle == null)
        {
            // Create a test vehicle if none exists
            var vehicleType = context.VehicleTypes.FirstOrDefault();
            var vehicleStatus = context.VehicleStatuses.FirstOrDefault(s => s.Name == "Active");
            var fuelType = context.FuelTypes.FirstOrDefault();

            if (vehicleType == null || vehicleStatus == null || fuelType == null)
            {
                Console.WriteLine("Missing vehicle reference data (VehicleType, Status, or FuelType), skipping GPS seed");
                return;
            }

            vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "TEST-GPS-001",
                Make = "Toyota",
                Model = "Hilux",
                Year = 2020,
                CurrentOdometer = 50000,
                IsActive = true,
                VehicleTypeId = vehicleType.Id,
                VehicleStatusId = vehicleStatus.Id,
                FuelTypeId = fuelType.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Vehicles.AddAsync(vehicle);
            await context.SaveChangesAsync();
            Console.WriteLine($"Created test vehicle: {vehicle.RegistrationNumber}");
        }

        // Create a trip with GPS data showing speeding violation
        var startTime = DateTime.UtcNow.AddMinutes(-30); // Recent data within last hour
        
        // Route: Farm to Town (Makhasaneni area - South Africa)
        // Starting point: -26.2041, 31.9369 (near Makhasaneni)
        var gpsLocations = new List<GPSLocation>();

        // Normal speed section (60 km/h) - First 10 minutes
        for (int i = 0; i < 10; i++)
        {
            gpsLocations.Add(new GPSLocation
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                TripLogId = null,
                Timestamp = startTime.AddMinutes(i),
                Latitude = -26.2041 + (i * 0.001),
                Longitude = 31.9369 + (i * 0.001),
                Speed = 55 + (i % 5), // 55-60 km/h
                Heading = 45,
                Altitude = 650,
                Odometer = (int)(vehicle.CurrentOdometer + (i * 1.0)),
                IsActive = i == 9, // Only last point is "current" active location
                CreatedAt = startTime.AddMinutes(i),
                UpdatedAt = startTime.AddMinutes(i)
            });
        }

        // SPEEDING VIOLATION SECTION (130 km/h) - Minutes 10-20
        for (int i = 10; i < 20; i++)
        {
            gpsLocations.Add(new GPSLocation
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                TripLogId = null,
                Timestamp = startTime.AddMinutes(i),
                Latitude = -26.2041 + (i * 0.002),
                Longitude = 31.9369 + (i * 0.002),
                Speed = 125 + (i % 10), // 125-135 km/h (SPEEDING!)
                Heading = 90,
                Altitude = 660,
                Odometer = (int)(vehicle.CurrentOdometer + 10 + ((i - 10) * 2.2)),
                IsActive = i == 19,
                CreatedAt = startTime.AddMinutes(i),
                UpdatedAt = startTime.AddMinutes(i)
            });
        }

        // Back to normal speed (70 km/h) - Minutes 20-30
        for (int i = 20; i < 30; i++)
        {
            gpsLocations.Add(new GPSLocation
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                TripLogId = null,
                Timestamp = startTime.AddMinutes(i),
                Latitude = -26.2041 + (i * 0.0015),
                Longitude = 31.9369 + (i * 0.0015),
                Speed = 68 + (i % 6), // 68-74 km/h
                Heading = 135,
                Altitude = 670,
                Odometer = (int)(vehicle.CurrentOdometer + 32 + ((i - 20) * 1.2)),
                IsActive = i == 29,
                CreatedAt = startTime.AddMinutes(i),
                UpdatedAt = startTime.AddMinutes(i)
            });
        }

        // Another speeding section (110 km/h) - Minutes 30-35
        for (int i = 30; i < 35; i++)
        {
            gpsLocations.Add(new GPSLocation
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                TripLogId = null,
                Timestamp = startTime.AddMinutes(i),
                Latitude = -26.2041 + (i * 0.0018),
                Longitude = 31.9369 + (i * 0.0018),
                Speed = 108 + (i % 8), // 108-116 km/h (SPEEDING!)
                Heading = 180,
                Altitude = 680,
                Odometer = (int)(vehicle.CurrentOdometer + 44 + ((i - 30) * 1.9)),
                IsActive = i == 34,
                CreatedAt = startTime.AddMinutes(i),
                UpdatedAt = startTime.AddMinutes(i)
            });
        }

        // Final section - slowing down (40 km/h) - Minutes 35-40
        for (int i = 35; i < 40; i++)
        {
            gpsLocations.Add(new GPSLocation
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicle.Id,
                TripLogId = null,
                Timestamp = startTime.AddMinutes(i),
                Latitude = -26.2041 + (i * 0.001),
                Longitude = 31.9369 + (i * 0.001),
                Speed = 45 - (i - 35), // 45-40 km/h (slowing down)
                Heading = 225,
                Altitude = 690,
                Odometer = (int)(vehicle.CurrentOdometer + 53.5 + ((i - 35) * 0.7)),
                IsActive = i == 39,
                CreatedAt = startTime.AddMinutes(i),
                UpdatedAt = startTime.AddMinutes(i)
            });
        }

        // Add all GPS locations
        await context.GPSLocations.AddRangeAsync(gpsLocations);
        await context.SaveChangesAsync();

        Console.WriteLine($"Seeded {gpsLocations.Count} GPS location records with speeding violations for vehicle {vehicle.RegistrationNumber}");
    }
}
