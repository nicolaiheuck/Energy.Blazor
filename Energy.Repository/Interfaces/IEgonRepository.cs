using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddReadingAsync(DataReading dataReading);
    Task<Location?> GetSchoolByNameAsync(string? schoolName);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<Location>> GetAllLocationsBySchoolNameAsync(string location);
}