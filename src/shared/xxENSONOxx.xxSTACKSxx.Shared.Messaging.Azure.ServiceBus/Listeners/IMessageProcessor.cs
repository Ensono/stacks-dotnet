﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Listeners
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(Message message, CancellationToken cancellationToken);
    }
}
