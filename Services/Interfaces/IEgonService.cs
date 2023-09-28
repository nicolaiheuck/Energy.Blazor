using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset = 0);
    Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, string floor, string room);
    Task AddReadingAsync(MQTTDataReadingDTO dataReadingDto, string school, string floor, string room);
    Task<List<TelemetryDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<TelemetryDTO>> GetAllDataReadingsByLocationIdAsync(LocationDTO locationDTO);
    Task<List<LocationDTO>> GetAllLocationsBySchoolAsync(string school);
    Task<List<LocationDTO>> GetAllRoomsByFloorAsync(string floor);
    Task<LocationDTO?> GetLocationIdBySchoolFloorRoomAsync(LocationDTO locationDTO);
    Task<List<TelemetryDTO>> GetAveragedTelemetryAsync(DateTime startDate, DateTime endDate, string schoolName, string? floor = null, bool byHour = false);
}