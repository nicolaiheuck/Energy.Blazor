using Mcc.Application.Features.Robotool.Commands;

namespace Mcc.Application.Contracts.Infrastructure
{
    public interface IMqttIotService
    {
        public event EventHandler<EventArgs>? SubscribeToTest;


        Task IotConnectAndSubscribeAsync(CancellationToken cancellationToken);

        // Publish Unload
        Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken);

    }
}
