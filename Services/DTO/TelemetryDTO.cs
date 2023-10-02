using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Services.DTO;

public class TelemetryDTO
{
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal KiloWattHour { get; set; }
    public int LocationId { get; set; }
    public DateTime SQLTStamp { get; set; } = DateTime.Now;

    // Navigation properties
    public LocationDTO Location { get; set; }
}