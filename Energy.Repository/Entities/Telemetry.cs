using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Repositories.Entities;

[Table("EGON_Telemetry")]
public class Telemetry
{
    public int Id { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public decimal KiloWattHour { get; set; }
    public decimal PeakKiloWatt { get; set; }
    public decimal KW_Day { get; set; }
    public decimal KW_YearSummarized { get; set; }
    public int LocationId { get; set; }
    public DateTime SQLTStamp { get; set; } = DateTime.Now;

    // Navigation properties
    public Location Location { get; set; }
}