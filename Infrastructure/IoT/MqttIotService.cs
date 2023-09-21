using Energy.Infrastructure.Mqtt.Services;
using Energy.Repositories;
using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Commands;
using MQTTnet.Client;
using System.Text.Json;

namespace Energy.Infrastructure.IoT
{
    [IgnoreService]
    public class MqttIotService : IMqttIotService
    {
        public event EventHandler<EventArgs>? SubscribeToEgonData;

        private readonly IMqttService _mqttService;
        private const string _BaseUrlPub = "energy/pub/";
        private const string _BaseUrlSub = "+/+/+/pv/";

        private const string _egonDataEndpointTopic = "data";

        public MqttIotService(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task IotConnectAndSubscribeAsync(CancellationToken cancellationToken)
        {
            if (!_mqttService.IsConnected())
            {
                await _mqttService.ConnectAsync(cancellationToken);
            }

            // read Message "func" from Mqtt subscriptions
            Func<MqttApplicationMessageReceivedEventArgs, Task> func = new(OnMessageReceivedIotAsync);
            _mqttService.HandleReceivedApplicationMessage(func);

            await _mqttService.SubscribeAsync(_BaseUrlSub + _egonDataEndpointTopic);

        }

        public Task OnMessageReceivedIotAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            if (args.ApplicationMessage.Topic.EndsWith("/pv/data"))
            {
                SubscribeToEgonData?.Invoke(this, args);
            }
            
            return Task.CompletedTask;
        }

        public Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken)
        {
            var payload = JsonSerializer.Serialize(command);
            return _mqttService.PublishAsync($"{_BaseUrlPub}{_egonDataEndpointTopic}", payload, cancellationToken);
        }
    }
}
