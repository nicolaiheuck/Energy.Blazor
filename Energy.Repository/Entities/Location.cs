using System.ComponentModel.DataAnnotations.Schema;

namespace Energy.Repositories.Entities;

[Table("EGON_Location")]
public class Location
{
    public int LocationId { get; set; }
    public string Room { get; set; }
    public int Floor { get; set; }
    public string School { get; set; }
}