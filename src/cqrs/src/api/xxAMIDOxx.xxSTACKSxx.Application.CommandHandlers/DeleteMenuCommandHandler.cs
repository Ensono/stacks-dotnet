using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

public class DeleteMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
    : ICommandHandler<DeleteMenu, bool>
{
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
            new MenuDeletedEvent(command, command.MenuId)
        );

        return successful;
    }
}
