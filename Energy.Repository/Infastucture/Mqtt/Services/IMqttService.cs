using MQTTnet.Client;

namespace Energy.Repositories.Infastucture.Mqtt.Services
{
    public interface IMqttService
    {
        void Initialize(MqttClientOptions mqttClientOptions);
        Task SubscribeAsync(string topic);
        Task UnsubscribeAsync(string topic);
        Task ConnectAsync(CancellationToken cancellationToken);
        bool IsConnected();
        Task PublishAsync(string topic, string payload, CancellationToken cancellationToken);
        void HandleReceivedApplicationMessage(Func<MqttApplicationMessageReceivedEventArgs, Task> func);
    }
}
