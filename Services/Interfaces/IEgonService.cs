using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset = 0);
    Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, string floor, string room);
    Task AddReadingAsync(MQTTDataReadingDTO dataReadingDto, string school, string floor, string room);
}