using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class FuelLogRepository : Repository<FuelLog>, IFuelLogRepository
{
    public FuelLogRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<IEnumerable<FuelLog>> GetByVehicleAsync(Guid vehicleId)
    {
        return await _context.FuelLogs
            .Include(f => f.Vehicle)
            .Include(f => f.FilledBy)
            .Where(f => f.VehicleId == vehicleId && f.IsActive)
            .OrderByDescending(f => f.FuelDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<FuelLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.FuelLogs
            .Include(f => f.Vehicle)
            .Include(f => f.FilledBy)
            .Where(f => f.FuelDate >= startDate && f.FuelDate <= endDate && f.IsActive)
            .OrderByDescending(f => f.FuelDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalFuelCostAsync(Guid vehicleId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.FuelLogs
            .Where(f => f.VehicleId == vehicleId && f.IsActive);

        if (startDate.HasValue)
            query = query.Where(f => f.FuelDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(f => f.FuelDate <= endDate.Value);

        return await query.SumAsync(f => f.TotalCost);
    }

    public async Task<decimal> GetAverageFuelConsumptionAsync(Guid vehicleId)
    {
        var fuelLogs = await _context.FuelLogs
            .Where(f => f.VehicleId == vehicleId && f.IsFull && f.IsActive)
            .OrderBy(f => f.OdometerReading)
            .Select(f => new { f.OdometerReading, f.Quantity })
            .ToListAsync();

        if (fuelLogs.Count < 2)
            return 0;

        decimal totalDistance = 0;
        decimal totalFuel = 0;

        for (int i = 1; i < fuelLogs.Count; i++)
        {
            var distance = fuelLogs[i].OdometerReading - fuelLogs[i - 1].OdometerReading;
            if (distance > 0)
            {
                totalDistance += distance;
                totalFuel += fuelLogs[i].Quantity;
            }
        }

        return totalDistance > 0 ? (totalFuel / totalDistance) * 100 : 0;
    }

    public async Task<IEnumerable<FuelLog>> GetAnomalousConsumptionAsync(Guid vehicleId)
    {
        var averageConsumption = await GetAverageFuelConsumptionAsync(vehicleId);
        var threshold = averageConsumption * 1.2m; // 20% above average

        var fuelLogs = await _context.FuelLogs
            .Include(f => f.Vehicle)
            .Include(f => f.FilledBy)
            .Where(f => f.VehicleId == vehicleId && f.IsActive)
            .OrderBy(f => f.OdometerReading)
            .ToListAsync();

        var anomalous = new List<FuelLog>();

        for (int i = 1; i < fuelLogs.Count; i++)
        {
            var distance = fuelLogs[i].OdometerReading - fuelLogs[i - 1].OdometerReading;
            if (distance > 0)
            {
                var consumption = (fuelLogs[i].Quantity / distance) * 100;
                if (consumption > threshold)
                {
                    anomalous.Add(fuelLogs[i]);
                }
            }
        }

        return anomalous;
    }
}
