namespace Mcc.Infrastructure.Mqtt.Configuration
{
    public class MqttConfig
    {
        public const string MqttSection = "Mqtt";
        public string BaseUrl { get; set; } = string.Empty;
        public string EnergyOnlineState { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
