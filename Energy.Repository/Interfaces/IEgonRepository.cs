using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddTemperatureReadingAsync(DataReading dataReading);
    Task<Location?> GetSchoolByNameAsync(string? schoolName);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task AddPowerReadingAsync(PowerReading powerReading, string school, string floor, string room);
}