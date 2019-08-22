using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Core.Exceptions;
using Amido.Stacks.Domain;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public abstract class MenuCommandHandlerBase<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : IMenuCommand
    {
        protected IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public MenuCommandHandlerBase(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            TResult result = default(TResult);

            var menu = await repository.GetByIdAsync(command.MenuId);

            if (menu == null)
                MenuDoesNotExistException.Raise(command, command.MenuId);

            //TODO: Check if the user has permission(Is the owner of the resource) 
            // to do after OIDC is setup, requires design

            try
            {
                result = await HandleCommandAsync(menu, command);

                var issuccessful = await repository.SaveAsync(menu);

                if (!issuccessful)
                    throw new Exception("Unable to complete operation");

                foreach (var appEvent in RaiseApplicationEvents(menu, command))
                {
                    await applicationEventPublisher.PublishAsync(appEvent);
                }
            }
            catch (DomainException ex)
            {
                DomainRuleViolationException.Raise(command, command.MenuId, ex);
            }
            catch (ApplicationExceptionBase ex)
            {
                //TODO: Change to an applicaiton exception handling
                //poossible failures is calling external dependencies like other services
                DomainRuleViolationException.Raise(command, command.MenuId, ex);
            }
            catch (InfrastructureExceptionBase ex)
            {
                //TODO: Change to an infrastructure exception handling
                //possible failures is calling database, queue or any other dependency
                DomainRuleViolationException.Raise(command, command.MenuId, ex);
            }
            catch (Exception ex)
            {
                ex.Data["OperationCode"] = command.OperationCode;
                ex.Data["CorrelationId"] = command.CorrelationId;
                ex.Data["MenuId"] = command.MenuId;

                throw ex;
            }


            return result;
        }

        /// <summary>
        /// The bae command handler will pre-load the aggregate root and provide it to the command handler with the command
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract Task<TResult> HandleCommandAsync(Menu menu, TCommand command);

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
