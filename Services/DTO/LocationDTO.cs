using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Services.DTO;

[Table("EGON_Location")]
public class LocationDTO
{
    public int LocationId { get; set; }
    public string Room { get; set; }
    public int Floor { get; set; }
    public string School { get; set; }
}