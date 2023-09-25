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
    
    public async Task AddReadingAsync(DataReading dataReading)
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

    public async Task<List<Location>> GetAllLocationsBySchoolNameAsync(string location)
    {
        return await _context.Locations
            .Where(d => d.School == location)
            .ToListAsync();
    }
}