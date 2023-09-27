namespace Energy.Services.DTO;

public class FagDTO
{
    public string Description { get; set; }
    public DateTime ClassStartdate { get; set; }
    public DateTime ClassEnddate { get; set; }
    public bool Remotestudy { get; set; }
    public int School { get; set; }
    public string ShortDescription { get; set; }
    public string Location { get; set; }
    public string LocationDescription { get; set; }
    public double HoursDay { get; set; }
    public string Education { get; set; }
}