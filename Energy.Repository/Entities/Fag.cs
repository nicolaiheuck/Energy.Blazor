namespace Energy.Repositories.Entities;

public class Fag
{
    public string betegnelse { get; set; }
    public string fag { get; set; }
    public bool fag_arves { get; set; }
    public string fag_id { get; set; }
    public DateTime fag_slutdato { get; set; }
    public DateTime fag_startdato { get; set; }
    public bool fjernundervisning { get; set; }
    public bool foregaar_undervisning_paa_virksomhed { get; set; }
    public double fuld_justeret_pris { get; set; }
    public int gennemfoerende_skole { get; set; }
    public bool godkendt_skole { get; set; }
    public double justeret_pris { get; set; }
    public string kort_betegnelse { get; set; }
    public string lms { get; set; }
    public int niveau { get; set; }
    public int norm_varighed { get; set; }
    public Periode perioder { get; set; }
    public int plan_varighed { get; set; }
    public double timer_pr_dag { get; set; }
    public string uddannelse { get; set; }
}