using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Events;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class DeleteMenuCommandHandler : ICommandHandler<DeleteMenu>
    {
        IMenuRepository repository;
        IApplicationEventPublisher applicationEventPublisher;

        public DeleteMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task HandleAsync(DeleteMenu command)
        {
            var menu = await repository.GetByIdAsync(command.Id);

            if (menu == null)
                MenuDoesNotExistException.Raise(OperationId.DeleteMenu, command.Id);

            //TODO: Check if the user has permission(Is the owner of the resource) 

            await repository.DeleteAsync(command.Id);

            await applicationEventPublisher.PublishAsync(new MenuDeletedEvent(command.Id));
        }
    }
}
