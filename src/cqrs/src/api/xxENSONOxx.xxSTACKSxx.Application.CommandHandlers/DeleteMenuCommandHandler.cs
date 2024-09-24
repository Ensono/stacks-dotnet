using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.Common.Exceptions;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

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
