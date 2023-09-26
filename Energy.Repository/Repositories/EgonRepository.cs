using Energy.Repositories.DbContexts;
using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Energy.Repositories.Repositories;

public class EgonRepository : IEgonRepository
{
    private readonly EgonContext _context;

    public EgonRepository(EgonContext context)
    {
        _context = context;
    }
    
    public async Task AddTemperatureReadingAsync(DataReading dataReading)
    {
        _context.Add(dataReading);
        await _context.SaveChangesAsync();
    }

    public async Task<Location?> GetSchoolByNameAsync(string? schoolName)
    {
        return await _context.Locations.FirstOrDefaultAsync(l => l.School == schoolName);
    }

    public async Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        return await _context.DataReadings
            .Where(d => d.SQLTStamp >= startTime && d.SQLTStamp <= endTime)
            .ToListAsync();
    }

    public async Task AddPowerReadingAsync(PowerReading powerReading, string school, string floor, string room)
    {
        var location = await _context.Locations.FirstAsync(l => l.School == school && l.Floor == floor && l.Room == room);
        powerReading.LocationId = location.LocationId; //NH_TODO: Move this logic into service once jan is done with his
        _context.Add(powerReading);
        await _context.SaveChangesAsync();
    }
}