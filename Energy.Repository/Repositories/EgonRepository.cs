using Energy.Repositories.DbContexts;
using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Energy.Repositories.Repositories;

public class EgonRepository : IEgonRepository
{
    private readonly EgonContext _context;
    private readonly HttpClient _httpClient;

    public EgonRepository(EgonContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task AddTemperatureReadingAsync(DataReading dataReading)
    {
        _context.Add(dataReading);
        await _context.SaveChangesAsync();
    }

    public async Task<Location?> FindSchoolLocationAsync(string[]? topicLocations)
    {
        return await _context.Locations.FirstOrDefaultAsync(l =>
            l.School == topicLocations[0] && l.Floor == topicLocations[1] && l.Room == topicLocations[2]);
    }
    
    public async Task<Location?> FindSchoolLocationAsync(string? schoolName)
    {
        return await _context.Locations.FirstOrDefaultAsync(l =>
            l.School == schoolName);
    }

    public async Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        return await _context.DataReadings
            .Where(d => d.SQLTStamp >= startTime && d.SQLTStamp <= endTime)
            .ToListAsync();
    }

    public async Task<List<Fag>> GetAllClassesOnSchoolAsync(int schoolId, int limit, int offset = 0)
    {
        // var apiResult = await _httpClient.GetFromJsonAsync<List<Fag>>(new Uri(
        //    $"https://ist-mock.tved.it/fag?limit={limit}&offset={offset}&instnr={schoolId}&apiKey=Vista%20Software'"));

        var apiResult = await _httpClient.PostAsync(
            new Uri(
                $"https://ist-mock.tved.it/fag?limit={limit}&offset={offset}&instnr={schoolId}&apiKey=Vista%20Software"),
            null); // TODO FIX URL TO API

        var debug = await apiResult.Content.ReadAsStringAsync();
        
        return await apiResult.Content.ReadFromJsonAsync<List<Fag>>() ?? new List<Fag>();
    }

    public async Task AddPowerReadingAsync(PowerReading powerReading, string school, string floor, string room)
    {
        //NH_TODO: Ensure works
        var location = await _context.Locations.FirstAsync(l => l.School == school && l.Floor == floor && l.Room == room);
        powerReading.KW_Day = _context.PowerReadings
            .Where(p => p.SQLTStamp.Date == DateTime.Today.Date)
            .Sum(p => p.KiloWattHour);
        powerReading.KW_YearSummarized = _context.PowerReadings
            .Where(p => p.SQLTStamp.Year == DateTime.Now.Year)
            .Sum(p => p.KiloWattHour);
        powerReading.LocationId = location.LocationId; //NH_TODO: Move this logic into service once jan is done with his
        _context.Add(powerReading);
        await _context.SaveChangesAsync();
    }
}