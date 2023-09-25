using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task AddReadingAsync(MQTTDataReadingDTO? dto, string? schoolName);
    Task<List<DataReadingDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<LocationDTO>> GetAllLocationsBySchoolNameAsync(string location);
}