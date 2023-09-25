using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Energy.Services.Services;

public class EgonService : IEgonService
{
    private readonly IEgonRepository _egonRepository;
    private readonly ILogger<EgonService> _logger;

    public EgonService(IEgonRepository egonRepository, ILogger<EgonService> logger)
    {
        _egonRepository = egonRepository;
        _logger = logger;
    }

    public async Task AddReadingAsync(MQTTDataReadingDTO? dto, string? schoolName)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dto);

            var school = await _egonRepository.GetSchoolByNameAsync(schoolName);
            ArgumentNullException.ThrowIfNull(school);

            DataReading dataReading = new()
            {
                LocationId = school.LocationId,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity
            };

            await _egonRepository.AddReadingAsync(dataReading);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddReadingAsync failed");
        }
    }

    public async Task<List<DataReadingDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        try
        {
            var results = await _egonRepository.GetAllDataReadingsAsync(startTime, endTime);
            List<DataReadingDTO> dataReadingDTOs = new ();
            foreach (var result in results)
            {
                LocationDTO locationDTO = new LocationDTO();
                locationDTO.School = result.Location.School;
                locationDTO.Floor = result.Location.Floor;
                locationDTO.Room = result.Location.Room;

                dataReadingDTOs.Add(new DataReadingDTO
                {
                    Temperature = result.Temperature,
                    Humidity = result.Humidity,
                    SQLTStamp = result.SQLTStamp,
                    Location = locationDTO

                });
            }
            
            return dataReadingDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllDataReadingsAsync failed");
            throw;
        }
    }

    public async Task<List<LocationDTO>> GetAllLocationsBySchoolNameAsync(string location)
    {
        var results = await _egonRepository.GetAllLocationsBySchoolNameAsync(location);
        List<LocationDTO> locationsDTO = new();
        foreach (var result in results)
        {
            locationsDTO.Add(new LocationDTO
            {
                School = result.School,
                Floor = result.Floor,
                Room = result.Room,
            });
        }
        return locationsDTO;
    }
}