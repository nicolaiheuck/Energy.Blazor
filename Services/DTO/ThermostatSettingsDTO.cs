namespace Energy.Services.DTO;

public class ThermostatSettingsDTO
{
    public string School { get; set; }
    public string Floor { get; set; }
    public string Room { get; set; }
    public string NewTemperature { get; set; }
    public string NewHysteresis { get; set; }
}