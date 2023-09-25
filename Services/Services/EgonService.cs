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
}