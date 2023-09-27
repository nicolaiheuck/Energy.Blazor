using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddPowerReadingAsync(PowerReading powerReading);
    Task AddTemperatureReadingAsync(DataReading dataReading);
    Task<Location?> FindSchoolLocationAsync(string school, string floor, string room);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<DataReading>> GetAllDataReadingsByLocationIdAsync(int locationId);
    Task<List<Location>> GetAllLocationsBySchoolAsync(string location);
    Task<List<Location>> GetAllRoomsByFloorAsync(string floor);
    Task<Location?> GetLocationIdBySchoolFloorRoomAsync(Location location);
    Task<List<Fag>> GetAllClassesOnSchoolAsync(int schoolId, int limit, int offset = 0);
}