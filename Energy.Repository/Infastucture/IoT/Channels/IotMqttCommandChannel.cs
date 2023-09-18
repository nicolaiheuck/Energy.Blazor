using Mcc.Application.Features.Robotool.Channels;
using Mcc.Application.Features.Robotool.Commands;
using System.Threading.Channels;

namespace Mcc.Infrastructure.Robotool.Channels
{
    public class IotMqttCommandChannel : IIotMqttCommandChannel
    {
        public ChannelReader<BaseIotCommand> Reader => _channel.Reader;

        private readonly Channel<BaseIotCommand> _channel;

        public IotMqttCommandChannel()
        {
            // Define and create the channel
            var options = new BoundedChannelOptions(50)
            {
                SingleWriter = false,
                SingleReader = true
            };

            _channel = Channel.CreateBounded<BaseIotCommand>(options);
        }


        public async Task<bool> AddCommandAsync(BaseIotCommand command, CancellationToken cancellationToken = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
            {
                if (_channel.Writer.TryWrite(command))
                {
                    return true;
                }
            }

            return false;
        }


        public IAsyncEnumerable<BaseIotCommand> ReadAllAsync(CancellationToken cancellationToken = default) =>
            _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
