using Energy.Services.DTO;

namespace Energy.Services.Interfaces;

public interface IEgonService
{
    Task AddReadingAsync(MQTTDataReadingDTO? dto, string? schoolName);
}