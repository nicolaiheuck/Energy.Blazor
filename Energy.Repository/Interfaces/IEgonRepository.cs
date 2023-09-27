using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddReadingAsync(DataReading dataReading);
    Task<Location?> FindSchoolLocationAsync(string[]? topicsLocation);
    Task<Location?> FindSchoolLocationAsync(string? schoolName);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<DataReading>> GetAllDataReadingsByLocationIdAsync(int locationId);
    Task<List<Location>> GetAllLocationsBySchoolAsync(string school);
    Task<List<Location>> GetAllRoomsByFloorAsync(int floor);
    Task<Location?> GetLocationIdBySchoolFloorRoomAsync(Location location);
    Task<List<Fag>> GetAllClassesOnSchoolAsync(int schoolId, int limit, int offset);
}