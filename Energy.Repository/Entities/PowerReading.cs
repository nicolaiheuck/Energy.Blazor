using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Repositories.Entities;

[Table("EGON_Power")]
public class PowerReading
{
    public int Id { get; set; }
    
    public decimal KiloWattHour { get; set; }
    
    public decimal PeakKiloWattHour { get; set; }

    public int LocationId { get; set; }

    public DateTime SQLTStamp { get; set; } = DateTime.Now;
}