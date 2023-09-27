using System.Text;
using System.Text.Json;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Channels;
using Energy.Services.Services.IoT.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;

namespace Energy.Services.Services.IoT.Workers
{
    public class MqttIotWorker : BackgroundService
    {
        private readonly IIotMqttCommandChannel _iotMqttCommandChannel;
        private readonly IMqttIotService _mqttIotService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MqttIotWorker> _logger;

        public MqttIotWorker(IIotMqttCommandChannel iotMqttCommandChannel, IMqttIotService mqttIotService, IServiceProvider serviceProvider, ILogger<MqttIotWorker> logger)
        {
            _iotMqttCommandChannel = iotMqttCommandChannel;
            _mqttIotService = mqttIotService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("{name} is starting.", nameof(MqttIotWorker));
            await _mqttIotService.IotConnectAndSubscribeAsync(cancellationToken);

            _mqttIotService.SubscribeToEgonData += OnEgonDataMessageReceived;
            _mqttIotService.SubscribeToLocationData += OnLocationDataMessageReceived;

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
            _logger.LogDebug("{name} is stopping.", nameof(MqttIotWorker));
            _mqttIotService.SubscribeToEgonData -= OnEgonDataMessageReceived;

            await base.StopAsync(cancellationToken);
        }
        
        private async void OnEgonDataMessageReceived(object? sender, EventArgs e)
        {
            try
            {
                var args = e as MqttApplicationMessageReceivedEventArgs;
                ArgumentNullException.ThrowIfNull(args);

                var topics = args.ApplicationMessage.Topic.Split("/");
                var school = args.ApplicationMessage.Topic.Split("/")[0];
                var floor = args.ApplicationMessage.Topic.Split("/")[1];
                var room = args.ApplicationMessage.Topic.Split("/")[2];

                var payload = Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment.ToArray());
                
                var dataReadingDTO = JsonSerializer.Deserialize<MQTTDataReadingDTO>(payload);
                _logger.LogDebug("Received message on topic {topic} with payload {@payload}", args.ApplicationMessage.Topic, dataReadingDTO);
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IEgonService>();

                await service.AddReadingAsync(dataReadingDTO, topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OnEgonDataMessageReceived failed");
            }
        }

        private async void OnLocationDataMessageReceived(object? sender, EventArgs e)
        {
            try
            {
                var args = e as MqttApplicationMessageReceivedEventArgs;
                ArgumentNullException.ThrowIfNull(args);

                var topics = args.ApplicationMessage.Topic.Split("/");
                var school = topics[0];
                var floor = topics[1];
                var room = topics[2];

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IEgonService>();
                
                var jsonString = "";
                if (topics.Contains("requestlocation"))
                {
                    jsonString = JsonSerializer.Serialize<FagDTO>(await service.GetRoomBookingInfoAsync(school, int.Parse(floor), int.Parse(room)));
                };
                await _mqttIotService.PublishAsync($"{school}/{floor}/{room}/location", jsonString, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OnEgonDataMessageReceived failed");
            }
        }
    }
}
