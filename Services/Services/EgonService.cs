using System.Text;
using System.Text.Json;
using Energy.Repositories.Entities;
using Energy.Repositories.Interfaces;
using Energy.Services.DTO;
using Energy.Services.Interfaces;

namespace Energy.Services.Services;

public class EgonService : IEgonService
{
    private readonly IEgonRepository _egonRepository;

    public EgonService(IEgonRepository egonRepository)
    {
        _egonRepository = egonRepository;
    }
    
    public async Task AddReadingAsync(MQTTDataReadingDTO? dto, string? schoolName)
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
}