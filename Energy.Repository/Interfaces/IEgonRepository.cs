using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddPowerReadingAsync(PowerReading powerReading, string school, string floor, string room);
    Task AddTemperatureReadingAsync(DataReading dataReading);
    Task<Location?> FindSchoolLocationAsync(string[]? topicsLocation);
    Task<Location?> FindSchoolLocationAsync(string? schoolName);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<Fag>> GetAllClassesOnSchoolAsync(int schoolId, int limit, int offset);
}