using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class DeleteMenuCommandHandler : ICommandHandler<DeleteMenu, bool>
    {
        readonly IMenuRepository repository;
        readonly IApplicationEventPublisher applicationEventPublisher;

        public DeleteMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<bool> HandleAsync(DeleteMenu command)
        {
            var menu = await repository.GetByIdAsync(command.MenuId);

            if (menu == null)
                MenuDoesNotExistException.Raise(command, command.MenuId);

            // TODO: Check if the user owns the resource before any operation
            // if(command.User.TenantId != menu.TenantId)
            // {
            //     throw NotAuthorizedException()
            // }

            var successful = await repository.DeleteAsync(command.MenuId);

            if (!successful) 
                OperationFailedException.Raise(command, command.MenuId, "Unable to delete menu");

            await applicationEventPublisher.PublishAsync(
                new MenuDeleted(command, command.MenuId)
            );

            return successful;
        }
    }
}
