using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task AddReadingAsync(MQTTDataReadingDTO? dto, string? schoolName);
    Task<List<DataReadingDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<DataReadingDTO>> GetAllDataReadingsByLocationIdAsync(LocationDTO locationDTO);
    Task<List<LocationDTO>> GetAllLocationsBySchoolAsync(string school);
    Task<List<LocationDTO>> GetAllRoomsByFloorAsync(int floor);
    Task<LocationDTO?> GetLocationIdBySchoolFloorRoomAsync(LocationDTO locationDTO);
    Task AddReadingAsync(MQTTDataReadingDTO? dto, string[]? topics);
    Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset = 0);
    Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, int floor, int room);

}