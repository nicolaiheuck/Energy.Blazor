using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task AddReadingAsync(MQTTDataReadingDTO? dto, string[]? topics);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset = 0);
    Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, int floor, int room);
}