using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Energy.Services.Services.IoT.Channels;
using Energy.Services.Services.IoT.Commands;

namespace Energy.Services.Services;

public class EgonService : IEgonService
{
    private readonly IEgonRepository _egonRepository;
    private readonly ILogger<EgonService> _logger;
    private readonly IIotMqttCommandChannel _commandChannel;

    public EgonService(IEgonRepository egonRepository, ILogger<EgonService> logger, IIotMqttCommandChannel commandChannel)
    {
        _egonRepository = egonRepository;
        _logger = logger;
        _commandChannel = commandChannel;
    }

    public async Task AddReadingAsync(MQTTDataReadingDTO dto, string schoolName, string floor, string room)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dto);

            var schoolLocation = await _egonRepository.FindSchoolLocationAsync(schoolName, floor, room);
            ArgumentNullException.ThrowIfNull(schoolLocation);

            Telemetry telemetry = new()
            {
                LocationId = schoolLocation.LocationId,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                KiloWattHour = dto.KiloWattHour,
                PeakKiloWatt = dto.PeakKiloWatt,
            };

            await _egonRepository.AddReadingAsync(telemetry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddReadingAsync failed");
            throw;
        }
    }

    public async Task<List<TelemetryDTO>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        try
        {
            var results = await _egonRepository.GetAllDataReadingsAsync(startTime, endTime);
            List<TelemetryDTO> dataReadingDTOs = new();
            foreach (var result in results)
            {
                LocationDTO locationDTO = new LocationDTO();
                locationDTO.School = result.Location.School;
                locationDTO.Floor = result.Location.Floor;
                locationDTO.Room = result.Location.Room;

                dataReadingDTOs.Add(new TelemetryDTO
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

    public async Task<List<TelemetryDTO>> GetAllDataReadingsByLocationIdAsync(LocationDTO locationDTO, DateTime? startDate, DateTime endDate)
    {
          locationDTO = await GetLocationIdBySchoolFloorRoomAsync(locationDTO);
          var results = await _egonRepository.GetAllDataReadingsByLocationIdAsync(locationDTO.LocationId, startDate, endDate);
          List<TelemetryDTO> dataReadingDTOs = new();
          foreach (var result in results)
          {
              dataReadingDTOs.Add(new TelemetryDTO
              { 
                  Temperature = result.Temperature, 
                  Humidity = result.Humidity, 
                  SQLTStamp = result.SQLTStamp
              });
          }

          return dataReadingDTOs;
    }

    public async Task<List<LocationDTO>> GetAllLocationsBySchoolAsync(string school)
    {
        try
        {
            var results = await _egonRepository.GetAllLocationsBySchoolAsync(school);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllLocationsBySchoolAsync failed");
            throw;
        }
    }

    public async Task<List<LocationDTO>> GetAllRoomsByFloorAsync(string floor)
    {
        var results = await _egonRepository.GetAllRoomsByFloorAsync(floor);
        List<LocationDTO> locationsDTOs = new();
        foreach (Location result in results)
        {
            locationsDTOs.Add(new LocationDTO
            {
                School = result.School,
                Floor = result.Floor,
                Room = result.Room
            });
        }

        return locationsDTOs;
    }

    public async Task<LocationDTO?> GetLocationIdBySchoolFloorRoomAsync(LocationDTO locationDTO)
    {
        Location location = new();
        location.School = locationDTO.School;
        location.Floor = locationDTO.Floor;
        location.Room = locationDTO.Room;

        var results = await _egonRepository.GetLocationIdBySchoolFloorRoomAsync(location);
        locationDTO.LocationId = results.LocationId;
        return locationDTO;
    }

    public async Task SetThermostatSettingsAsync(ThermostatSettingsDTO dto)
    {
        await _commandChannel.AddCommandAsync(new SetThermostatSettingsCommand
        {
            NewTemperature = dto.NewTemperature,
            NewHysteresis = dto.NewHysteresis,
            School = dto.School,
            Floor = dto.Floor,
            Room = dto.Room,
        });
    }

    public async Task<List<FagDTO>> GetAllClassesFromAPIAsync(int schoolId, int limit, int offset)
    {
        var fagList = await _egonRepository.GetAllClassesOnSchoolAsync(schoolId, limit, offset);
        var fagDTOs = new List<FagDTO>();
        foreach (var fag in fagList)
        {
            fagDTOs.Add(new FagDTO
            {
                Description = fag.fag,
                ClassStartdate = fag.fag_startdato,
                ClassEnddate = fag.fag_slutdato,
                Remotestudy = fag.fjernundervisning,
                School = fag.gennemfoerende_skole,
                ShortDescription = fag.betegnelse,
                Location = fag.perioder.lokation,
                LocationDescription = fag.perioder.lokation_betegnelse,
                HoursDay = fag.timer_pr_dag,
                Education = fag.uddannelse
            });
        }

        return fagDTOs;
    }

    public async Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, string floor, string room)
    {
        var schoolId = await _egonRepository.FindSchoolLocationAsync(schoolName, floor, room);
        var listOfClasses =
            await GetAllClassesFromAPIAsync(schoolId.LocationId, 10, 0); // DEBUG Hardcoded limit because of mock API
        var roomInfo = listOfClasses
            .Where(c => c.Location == $"{floor}.{room}")
            .MinBy(c => c.ClassStartdate);

        return roomInfo;
    }

    public async Task<List<TelemetryDTO>> GetAveragedTelemetryAsync(DateTime startDate, DateTime endDate, string schoolName, string? floor = null, bool byHour = false)
    {
        var locations = floor != null ? await _egonRepository.GetAllRoomsByFloorAsync(floor) : await _egonRepository.GetAllLocationsBySchoolAsync(schoolName);
        var readings = await _egonRepository.GetAveragedTelemetryAsync(startDate, endDate, locations, byHour);

        return readings.Select(r => new TelemetryDTO
        {
            Temperature = r.Temperature,
            Humidity = r.Humidity,
            LocationId = r.LocationId,
            SQLTStamp = r.SQLTStamp,
            KiloWattHour = r.KiloWattHour
        }).ToList();
    }
}