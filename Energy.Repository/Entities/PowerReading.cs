using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Repositories.Entities;

[Table("EGON_Power")]
public class PowerReading
{
    public int Id { get; set; }
    
    public decimal KiloWattHour { get; set; }
    
    public decimal PeakKiloWatt { get; set; }

    public int LocationId { get; set; }

    public DateTime SQLTStamp { get; set; } = DateTime.Now;

    public decimal KW_Day { get; set; }
    
    public decimal KW_YearSummarized { get; set; }
}