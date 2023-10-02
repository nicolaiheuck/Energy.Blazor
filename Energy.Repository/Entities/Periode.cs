namespace Energy.Repositories.Entities;

public class Periode
{
    public int fag_periode_id { get; set; }
    public DateTime fag_periode_slut { get; set; }
    public DateTime fag_periode_start { get; set; }
    public string lokation { get; set; }
    public string lokation_betegnelse { get; set; }
    public int norm_varighed_dage { get; set; }
    public int periode_varighed { get; set; }
    public double timer_pr_dag { get; set; }
    public string tmk { get; set; }
}