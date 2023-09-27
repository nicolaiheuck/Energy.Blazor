using Energy.Infrastructure.Mqtt.Configuration;
using Energy.Repositories;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Mqtt.Services
{
    [IgnoreService]
    public sealed class MqttService : IMqttService
    {
        private IManagedMqttClient? _mqttClient;
        private ManagedMqttClientOptions? _managedMqttClientOptions;
        private readonly IOptionsMonitor<MqttConfig> _mqttConfig;
        private readonly ILogger<MqttService> _logger;

        public MqttService(IOptionsMonitor<MqttConfig> mqttConfig, ILogger<MqttService> logger)
        {
            _mqttConfig = mqttConfig;
            _logger = logger;

            Initialize(new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithCleanSession(true)
                .WithTcpServer(_mqttConfig.CurrentValue.BaseUrl)
                .WithCredentials(_mqttConfig.CurrentValue.Username, _mqttConfig.CurrentValue.Password)
                .WithWillTopic(_mqttConfig.CurrentValue.EnergyOnlineState)
                .WithWillPayload("offline")
                .WithWillRetain(true)
                .Build());
        }


        public void Initialize(MqttClientOptions mqttClientOptions)
        {
            _managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();
            var factory = new MqttFactory();
            _mqttClient = factory.CreateManagedMqttClient();
        }


        public bool IsConnected()
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);

            return _mqttClient.IsConnected;
        }


        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    await _mqttClient.StartAsync(_managedMqttClientOptions);
                    var message = new MqttApplicationMessage()
                    {
                        Topic = _mqttConfig.CurrentValue.EnergyOnlineState,
                        PayloadSegment = Encoding.UTF8.GetBytes("online"),
                        Retain = true,
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                    };
                    await _mqttClient.EnqueueAsync(message);
                    _logger.LogInformation("{name} is started.", nameof(MqttService));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("MQTT client failed to connect. Exception message: {errorMessage}", ex.Message);
            }
        }


        public void HandleReceivedApplicationMessage(Func<MqttApplicationMessageReceivedEventArgs, Task> func)
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);
            _mqttClient.ApplicationMessageReceivedAsync += func;
        }


        private async Task EnqueueAsync(MqttApplicationMessage message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);
            try
            {
                await ConnectAsync(cancellationToken);
                await _mqttClient.EnqueueAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("MQTT client failed to Publish. Exception message: {errorMessage}", ex.Message);
            }
        }


        private static MqttApplicationMessage ConvertToMqttMessage(string topic, byte[] payload, bool retain)
        {
            return new MqttApplicationMessage()
            {
                Topic = topic,
                PayloadSegment = payload,
                Retain = retain,
                QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
            };
        }


        public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken, bool retain = false)
        {
            var message = ConvertToMqttMessage(topic, Encoding.UTF8.GetBytes(payload), retain);
            await EnqueueAsync(message, cancellationToken);
        }


        public async Task SubscribeAsync(string topic)
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);
            try
            {
                await _mqttClient.SubscribeAsync(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError("MQTT client failed to Subscribe. Exception message: {errorMessage}", ex.Message);
            }
        }


        public async Task UnsubscribeAsync(string topic)
        {
            ArgumentNullException.ThrowIfNull(_mqttClient);
            try
            {
                await _mqttClient.UnsubscribeAsync(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError("MQTT client failed to Unsubscribe. Exception message: {errorMessage}", ex.Message);
            }
        }
    }
}
