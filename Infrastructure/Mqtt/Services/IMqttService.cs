﻿using Energy.Repositories;
using MQTTnet.Client;

namespace Energy.Infrastructure.Mqtt.Services
{
    [IgnoreService]
    public interface IMqttService
    {
        void Initialize(MqttClientOptions mqttClientOptions);
        Task SubscribeAsync(string topic);
        Task UnsubscribeAsync(string topic);
        Task ConnectAsync(CancellationToken cancellationToken);
        bool IsConnected();
        Task PublishAsync(string topic, string payload, CancellationToken cancellationToken, bool retain = false);
        void HandleReceivedApplicationMessage(Func<MqttApplicationMessageReceivedEventArgs, Task> func);
    }
}
