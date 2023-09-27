using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.Extensions.Logging;

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

    public async Task AddReadingAsync(MQTTDataReadingDTO? dto, string[]? topics)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(dto);

            var school = await _egonRepository.FindSchoolLocationAsync(topics);
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

    public async Task<List<DataReading>> GetAllDataReadingsAsync(DateTime startTime, DateTime endTime)
    {
        try
        {
            var readings = await _egonRepository.GetAllDataReadingsAsync(startTime, endTime);
            return readings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllDataReadingsAsync failed");
            throw;
        }
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

    public async Task<FagDTO> GetRoomBookingInfoAsync(string schoolName, int floor, int room)
    {
        var schoolId = await _egonRepository.FindSchoolLocationAsync(schoolName);
        var listOfClasses = await GetAllClassesFromAPIAsync(schoolId.LocationId, 10, 0); // DEBUG Hardcoded limit because of mock API
        var roomInfo = listOfClasses
            .Where(c => c.Location == $"{floor}.{room}")
            .MinBy(c => c.ClassStartdate);

        return roomInfo;
    }
}