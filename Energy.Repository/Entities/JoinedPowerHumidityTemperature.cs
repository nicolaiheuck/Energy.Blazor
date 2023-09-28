namespace Energy.Repositories.Entities;

public class JoinedPowerHumidityTemperature
{
    public int LocationId { get; set; }
    public decimal Temperature { get; set; }
    public decimal Humidity { get; set; }
    public DateTime SQLTStamp { get; set; }
    public decimal KiloWattHour { get; set; }
}