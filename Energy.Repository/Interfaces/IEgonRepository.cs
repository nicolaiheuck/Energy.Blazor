using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddReadingAsync(Telemetry telemetry);
    Task<Location?> FindSchoolLocationAsync(string school, string floor, string room);
    Task<List<Telemetry>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<Telemetry>> GetAllDataReadingsByLocationIdAsync(int locationId);
    Task<List<Location>> GetAllLocationsBySchoolAsync(string location);
    Task<List<Location>> GetAllRoomsByFloorAsync(string floor);
    Task<Location?> GetLocationIdBySchoolFloorRoomAsync(Location location);
    Task<List<Fag>> GetAllClassesOnSchoolAsync(int schoolId, int limit, int offset = 0);
    Task<List<Telemetry>> GetAveragedTelemetryAsync(DateTime startDate, DateTime endDate, List<Location> locationsInSchool, bool byHour = false);
}