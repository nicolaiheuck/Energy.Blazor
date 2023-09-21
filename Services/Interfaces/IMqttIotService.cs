using Energy.Repositories;
using Energy.Services.Services.IoT.Commands;

namespace Energy.Services.Interfaces
{
    [IgnoreService]
    public interface IMqttIotService
    {
        public event EventHandler<EventArgs>? SubscribeToEgonData;


        Task IotConnectAndSubscribeAsync(CancellationToken cancellationToken);

        Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken);

    }
}
