using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task AddReadingAsync(MQTTDataReadingDTO dataReadingDto, string schoolName, string floor, string room);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
}