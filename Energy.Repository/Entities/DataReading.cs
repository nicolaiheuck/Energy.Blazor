using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Repositories.Entities;

[Table("EGON_Data")]
public class DataReading
{
    public int Id { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public int LocationId { get; set; }
    public DateTime SQLTStamp { get; set; } = DateTime.Now;
    
    // Navigation properties
    public Location Location { get; set; }
}