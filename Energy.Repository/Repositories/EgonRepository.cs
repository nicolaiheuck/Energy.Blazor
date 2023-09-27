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

    public async Task AddReadingAsync(DataReading dataReading)
    {
        _context.Add(dataReading);
        await _context.SaveChangesAsync();
    }


    public async Task<Location?> GetSchoolByNameAsync(string? schoolName)
    
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


    public async Task<List<Location>> GetAllLocationsBySchoolAsync(string location)
    {
        return await _context.Locations
            .Where(d => d.School == location)
            .ToListAsync();
    }


    public async Task<Location?> GetLocationIdBySchoolFloorRoomAsync(Location location)
    {
          return await _context.Locations.FirstOrDefaultAsync(d => d.School == location.School && d.Floor == location.Floor && d.Room == location.Room);
    }
    

    public async Task<List<Location>> GetAllRoomsByFloorAsync(int floor)
    {
      return await _context.Locations
         .Where(d => d.Floor == floor)
         .ToListAsync();
    }


    public async Task<List<DataReading>> GetAllDataReadingsByLocationIdAsync(int locationId)
    {
      return await _context.DataReadings
        .Where(d => d.LocationId == locationId)
        .ToListAsync();
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
}