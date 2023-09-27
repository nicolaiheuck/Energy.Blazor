using System.Text;
using Energy.Infrastructure.Mqtt.Services;
using Energy.Repositories;
using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Commands;
using MQTTnet.Client;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.IoT
{
    [IgnoreService]
    public class MqttIotService : IMqttIotService
    {
        public event EventHandler<EventArgs>? SubscribeToEgonData;
        public event EventHandler<EventArgs>? SubscribeToLocationData;

        private readonly IMqttService _mqttService;
        private readonly ILogger<MqttIotService> _logger;
        private const string _BaseUrlPub = "energy/pub/";
        private const string _BaseUrlSub = "+/+/+/pv/#";

        private const string _egonDataEndpointTopic = "data";

        public MqttIotService(IMqttService mqttService, ILogger<MqttIotService> logger)
        {
            _mqttService = mqttService;
            _logger = logger;
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

            await _mqttService.SubscribeAsync(_BaseUrlSub);
        }

        public Task OnMessageReceivedIotAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            if (args.ApplicationMessage.Topic.EndsWith("/pv/data"))
            {
                SubscribeToEgonData?.Invoke(this, args);
            }
            else if (args.ApplicationMessage.Topic.EndsWith("/location"))
            {
                SubscribeToLocationData?.Invoke(this, args);
            }
            else
            {
                var payload = Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment.ToArray());
                _logger.LogWarning("Received MQTT message on topic {topic} but doesn't know how to process it. Payload: {payload}", args.ApplicationMessage.Topic, payload);
            }
            
            return Task.CompletedTask;
        }

        public Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken)
        {
            var payload = JsonSerializer.Serialize(command);
            return _mqttService.PublishAsync($"{_BaseUrlPub}{_egonDataEndpointTopic}", payload, cancellationToken);
        }

        public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken)
        {
            await _mqttService.PublishAsync(topic, payload, cancellationToken);
        }
    }
}
