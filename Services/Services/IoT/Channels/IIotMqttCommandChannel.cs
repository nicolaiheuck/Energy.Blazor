using System.Threading.Channels;
using Energy.Services.Services.IoT.Commands;

namespace Energy.Services.Services.IoT.Channels
{
    public interface IIotMqttCommandChannel
    {
        ChannelReader<BaseIotCommand> Reader { get; }

        Task<bool> AddCommandAsync(BaseIotCommand command, CancellationToken cancellationToken = default);
        IAsyncEnumerable<BaseIotCommand> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
