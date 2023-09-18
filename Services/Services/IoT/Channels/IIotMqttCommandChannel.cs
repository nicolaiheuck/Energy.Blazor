using System.Threading.Channels;
using Mcc.Application.Features.Robotool.Commands;

namespace Mcc.Application.Features.Robotool.Channels
{
    public interface IIotMqttCommandChannel
    {
        ChannelReader<BaseIotCommand> Reader { get; }

        Task<bool> AddCommandAsync(BaseIotCommand command, CancellationToken cancellationToken = default);
        IAsyncEnumerable<BaseIotCommand> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
