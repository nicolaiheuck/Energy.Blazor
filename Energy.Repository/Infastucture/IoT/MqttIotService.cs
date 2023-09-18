using Mcc.Application.Contracts.Infrastructure;
using Mcc.Application.Features.Robotool.Commands;
using Mcc.Infrastructure.Mqtt.Services;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

namespace Mcc.Infrastructure.Robotool
{
    public class MqttIotService : IMqttIotService
    {
        // Unload
        public event EventHandler<EventArgs>? SubscribeToTest;


        private readonly IMqttService _mqttService;
        private const string _BaseUrlPub = "energy/pub/";
        private const string _BaseUrlSub = "energy/sub/";

        private const string _testEndpointTopic = "test";



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

            await _mqttService.SubscribeAsync(_BaseUrlSub + _testEndpointTopic);

        }


        public Task OnMessageReceivedIotAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            switch (args.ApplicationMessage.Topic)
            {
                // Unload
                case _BaseUrlSub + _testEndpointTopic:
                    //_logger.LogDebug($"Received MQTT message on topic:: {args.ApplicationMessage.Topic}, with payload: {Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment)}");
                    SubscribeToTest?.Invoke(this, new EventArgs());
                    break;

                default:
                    //_logger.LogWarning($"No Handerler for received MQTT message on topic:: {args.ApplicationMessage.Topic}, with payload: {Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment)}");
                    break;
            }

            return Task.CompletedTask;
        }



        public Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken)
        {
            var payload = JsonSerializer.Serialize(command);
            return _mqttService.PublishAsync($"{_BaseUrlPub}{_testEndpointTopic}", payload, cancellationToken);
        }
    }
}
