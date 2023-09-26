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
}