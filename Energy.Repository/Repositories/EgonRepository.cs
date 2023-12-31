using System.Net.Http.Json;
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

    public async Task AddReadingAsync(Telemetry telemetry)
    {
        telemetry.KW_Day = _context.Telemetry
                                   .AsNoTracking()
                                   .Where(p => p.SQLTStamp.Date == DateTime.Today.Date)
                                   .Sum(p => p.KiloWattHour);

        telemetry.KW_YearSummarized = _context.Telemetry
                                              .AsNoTracking()
                                              .Where(p => p.SQLTStamp.Year == DateTime.Now.Year)
                                              .Sum(p => p.KiloWattHour);
        _context.Add(telemetry);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Location?> FindSchoolLocationAsync(string school, string floor, string room)
    {
        return await _context.Locations.AsNoTracking().FirstOrDefaultAsync(l =>
            l.School == school && l.Floor == floor && l.Room == room);
    }

    public async Task<List<Location>> GetAllLocationsBySchoolAsync(string location)
    {
        return await _context.Locations
            .AsNoTracking()
            .Where(d => d.School == location)
            .ToListAsync();
    }

    public async Task<Location?> GetLocationIdBySchoolFloorRoomAsync(Location location)
    {
        return await _context.Locations.AsNoTracking().FirstOrDefaultAsync(d =>
            d.School == location.School && d.Floor == location.Floor && d.Room == location.Room);
    }

    public async Task<List<Location>> GetAllRoomsByFloorAsync(string floor)
    {
        return await _context.Locations
            .AsNoTracking()
            .Where(d => d.Floor == floor)
            .ToListAsync();
    }
    
    public async Task<List<Telemetry>> GetAllDataReadingsByLocationIdAsync(int locationId, DateTime? startDate, DateTime endDate)
    {
      return await _context.Telemetry
        .AsNoTracking()
        .Where(d => d.LocationId == locationId && d.SQLTStamp >= startDate && d.SQLTStamp <= endDate)
        .ToListAsync();
    }
    
    public async Task<List<Telemetry>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        return await _context.Telemetry
            .AsNoTracking()
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

    public async Task<List<Telemetry>> GetAveragedTelemetryAsync(DateTime startDate, DateTime endDate, List<Location> locationsInSchool, bool byHour = false)
    {
        var locationIds = locationsInSchool.Select(l => l.LocationId).ToList();
        return await _context.Telemetry
                             .AsNoTracking()
                             .Where(d => locationIds.Contains(d.LocationId))
                             .Where(d => d.SQLTStamp > startDate && d.SQLTStamp < endDate)
                             .GroupBy(d => new { Date = d.SQLTStamp.Date, Hour = byHour ? d.SQLTStamp.Hour : 0 })
                             .Select(grouping => new Telemetry
                             {
                                 SQLTStamp = grouping.Key.Date.AddHours(grouping.Key.Hour),
                                 Temperature = grouping.Average(r => r.Temperature),
                                 Humidity = grouping.Average(r => r.Humidity),
                                 KiloWattHour = grouping.Average(r => r.KiloWattHour)
                             })
                             .ToListAsync();
    }
}