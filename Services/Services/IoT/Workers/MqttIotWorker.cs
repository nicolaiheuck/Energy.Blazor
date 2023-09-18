using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Channels;
using Energy.Services.Services.IoT.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Energy.Services.Services.IoT.Workers
{
    public class MqttIotWorker : BackgroundService
    {
        private readonly IIotMqttCommandChannel _iotMqttCommandChannel;
        private readonly IMqttIotService _mqttIotService;

        public MqttIotWorker(IIotMqttCommandChannel iotMqttCommandChannel, IMqttIotService mqttIotService)
        {
            _iotMqttCommandChannel = iotMqttCommandChannel;
            _mqttIotService = mqttIotService;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("{name} is starting.", nameof(MqttRobotoolWorker));
            await _mqttIotService.IotConnectAndSubscribeAsync(cancellationToken);


            _mqttIotService.SubscribeToTest += OnTestMessageRecived;

            await foreach (var message in _iotMqttCommandChannel.ReadAllAsync(cancellationToken))
            {
                Task messageTask = message switch
                {
                    TestCommand msg => _mqttIotService.PublishTestAsync(msg, cancellationToken),
                    _ => throw new NotSupportedException()
                };
                await messageTask;
            }
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("{name} is stopping.", nameof(MqttRobotoolWorker));
            _mqttIotService.SubscribeToTest -= OnTestMessageRecived;

            await base.StopAsync(cancellationToken);
        }


        private void OnTestMessageRecived(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
