using System.Text.Json.Serialization;

namespace Energy.Services.DTO;

public class MQTTDataReadingDTO
{
    [JsonPropertyName("temperature")]
    public decimal Temperature { get; set; }
    
    [JsonPropertyName("humidity")]
    public decimal Humidity { get; set; }
    
    public decimal WattHour { get; set; }
}