using System.Text;
using System.Text.Json;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Channels;
using Energy.Services.Services.IoT.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;

namespace Energy.Services.Services.IoT.Workers
{
    public class MqttIotWorker : BackgroundService
    {
        private readonly IIotMqttCommandChannel _iotMqttCommandChannel;
        private readonly IMqttIotService _mqttIotService;
        private readonly IServiceProvider _serviceProvider;

        public MqttIotWorker(IIotMqttCommandChannel iotMqttCommandChannel, IMqttIotService mqttIotService, IServiceProvider serviceProvider)
        {
            _iotMqttCommandChannel = iotMqttCommandChannel;
            _mqttIotService = mqttIotService;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("{name} is starting.", nameof(MqttRobotoolWorker));
            await _mqttIotService.IotConnectAndSubscribeAsync(cancellationToken);


            _mqttIotService.SubscribeToEgonData += OnEgonDataMessageReceived;

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
            _mqttIotService.SubscribeToEgonData -= OnEgonDataMessageReceived;

            await base.StopAsync(cancellationToken);
        }


        private async void OnEgonDataMessageReceived(object? sender, EventArgs e)
        {
            var args = e as MqttApplicationMessageReceivedEventArgs;
            ArgumentNullException.ThrowIfNull(args);

            var school = args.ApplicationMessage.Topic.Split("/").FirstOrDefault();

            var payload = Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment.ToArray());
            var dataReadingDTO = JsonSerializer.Deserialize<MQTTDataReadingDTO>(payload);

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IEgonService>();
            
            await service.AddReadingAsync(dataReadingDTO, school);
        }
    }
}
