using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public abstract class MenuCommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : IMenuCommand
    {
        protected IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public MenuCommandHandlerBase(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task HandleAsync(TCommand command)
        {
            var menu = await repository.GetByIdAsync(command.MenuId);

            if (menu == null)
                MenuDoesNotExistException.Raise((OperationCode)command.OperationCode, command.CorrelationId, command.MenuId);

            //TODO: Check if the user has permission(Is the owner of the resource) 
            // to do after OIDC is setup, requires design

            await HandleCommandAsync(menu, command);

            foreach (var appEvent in RaiseApplicationEvents(menu, command))
            {
                await applicationEventPublisher.PublishAsync(appEvent);
            }
        }

        /// <summary>
        /// The bae command handler will pre-load the aggregate root and provide it to the command handler with the command
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract Task HandleCommandAsync(Menu menu, TCommand command);

        /// <summary>
        /// Application events that should be raised when the command succeed.
        /// Implement this method send application events to the message bus.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="command"></param>
        /// <returns>Application events to be published in the message bus</returns>
        public abstract IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, TCommand command);
    }
}
