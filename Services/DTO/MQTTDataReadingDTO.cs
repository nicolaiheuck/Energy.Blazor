using System.Text.Json.Serialization;

namespace Energy.Services.DTO;

public class MQTTDataReadingDTO
{
    [JsonPropertyName("temperature")]
    public decimal Temperature { get; set; }
    
    [JsonPropertyName("humidity")]
    public decimal Humidity { get; set; }
    
    [JsonPropertyName("kiloWattHour")]
    public decimal KiloWattHour { get; set; }
    
    [JsonPropertyName("peakKiloWattHour")]
    public decimal PeakKiloWattHour { get; set; }
}