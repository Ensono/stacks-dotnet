using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Core.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    /// <summary>
    /// MenuCommandHandlerBase used for common operations updating an existing menu
    /// Creation and Deletion of the AggregateRoot should have it's own implementation
    /// Command specific logic should be handled within HandleCommandAsync() method implementation
    /// </summary>
    /// <typeparam name="TCommand">The type of command being handled</typeparam>
    /// <typeparam name="TResult">The type of result expected. Use bool when no return value is expected</typeparam>
    public abstract class MenuCommandHandlerBase<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : IMenuCommand
    {
        protected IMenuRepository repository;
        private readonly IApplicationEventPublisher applicationEventPublisher;

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

            // TODO: Check if the user owns the resource before any operation
            // if(command.User.TenantId != menu.TenantId)
            // {
            //     throw NotAuthorizedException()
            // }

            try
            {
                result = await HandleCommandAsync(menu, command);

                var issuccessful = await repository.SaveAsync(menu);

                if (!issuccessful)
                    OperationFailedException.Raise(command, command.MenuId, 
                        $"Unable to handle command {typeof(ICommand).Name}");

                foreach (var appEvent in RaiseApplicationEvents(menu, command))
                {
                    await applicationEventPublisher.PublishAsync(appEvent);
                }
            }
            catch (DomainExceptionBase ex)
            {
                DomainRuleViolationException.Raise(command, command.MenuId, ex);
            }
            catch (ApplicationExceptionBase)
            {
                // TODO: handle applicaiton exception handling
                // possible failures is missing data or information, validations, and so on
                throw;
            }
            catch (InfrastructureExceptionBase)
            {
                //TODO: handle  infrastructure exception handling
                //possible failures is calling database, queue or any other dependency
                throw;
            }
            catch (Exception ex)
            {
                //TODO: Enrich the exception details with context information to track in the logs
                ex.Data["OperationCode"] = command.OperationCode;
                ex.Data["CorrelationId"] = command.CorrelationId;
                ex.Data["MenuId"] = command.MenuId;

                throw;
            }

            return result;
        }

        /// <summary>
        /// The base command handler will pre-load the aggregate root and provide it to the command handler with the command
        /// </summary>
        /// <param name="menu">the menu loaded from the repository</param>
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
