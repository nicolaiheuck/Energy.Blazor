using System.Text.Json.Serialization;
using Energy.Repositories;

namespace Energy.Services.Services.IoT.Commands;

[IgnoreService]
public class SetThermostatSettingsCommand : BaseIotCommand
{
    [JsonIgnore]
    public string School { get; set; }
    [JsonIgnore]
    public string Floor { get; set; }
    [JsonIgnore]
    public string Room { get; set; }
    
    public string NewTemperature { get; set; }
    public string NewHysteresis { get; set; }
}