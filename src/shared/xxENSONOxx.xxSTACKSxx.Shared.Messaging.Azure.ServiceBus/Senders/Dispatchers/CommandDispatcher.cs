using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Routers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders.Dispatchers
{
    // TODO: This will become MagicRouterLogicToWithCustomStrategies

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILogger<CommandDispatcher> _log;
        private readonly ServiceBusAbstractRouter<IQueueRouter> routing;

        public CommandDispatcher(
            ILogger<CommandDispatcher> log,
            ServiceBusAbstractRouter<IQueueRouter> routing
        )
        {
            this._log = log;
            this.routing = routing;
        }
        public async Task SendAsync(ICommand command)
        {
            _log.LogInformation($"Dispatching command {command.CorrelationId}");

            await routing.RouteAsync(command);

            _log.LogInformation($"{command.CorrelationId} has been processed");
        }
    }
}
