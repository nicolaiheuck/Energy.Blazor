﻿using Energy.Services.Services.IoT.Commands;

namespace Energy.Services.Interfaces
{
    public interface IMqttIotService
    {
        public event EventHandler<EventArgs>? SubscribeToTest;


        Task IotConnectAndSubscribeAsync(CancellationToken cancellationToken);

        // Publish Unload
        Task PublishTestAsync(TestCommand command, CancellationToken cancellationToken);

    }
}
