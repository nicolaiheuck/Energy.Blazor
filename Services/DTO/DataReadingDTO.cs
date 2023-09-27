using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Services.DTO;

[Table("EGON_Data")]
public class DataReadingDTO
{
    public int Id { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public int LocationId { get; set; }
    public DateTime SQLTStamp { get; set; } = DateTime.Now;

    // Navigation properties
    public LocationDTO Location { get; set; }
}