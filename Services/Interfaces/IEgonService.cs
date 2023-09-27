using Energy.Repositories.Entities;
using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset = 0);
    Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, string floor, string room);
    Task AddReadingAsync(MQTTDataReadingDTO dataReadingDto, string school, string floor, string room);
    Task<List<DataReadingDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime);
    Task<List<DataReadingDTO>> GetAllDataReadingsByLocationIdAsync(LocationDTO locationDTO);
    Task<List<LocationDTO>> GetAllLocationsBySchoolAsync(string school);
    Task<List<LocationDTO>> GetAllRoomsByFloorAsync(string floor);
    Task<LocationDTO?> GetLocationIdBySchoolFloorRoomAsync(LocationDTO locationDTO);
}