using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class CreateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
    : ICommandHandler<CreateMenu, Guid>
{
    public async Task<Guid> HandleAsync(CreateMenu command)
    {
        var id = Guid.NewGuid();

        // TODO: Check if the user owns the resource before any operation
        // if(command.User.TenantId != menu.TenantId)
        // {
        //     throw NotAuthorizedException()
        // }


        var newMenu = new Menu(
            id: id,
            name: command.Name,
            tenantId: command.TenantId,
            description: command.Description,
            categories: null,
            enabled: command.Enabled
        );

        await repository.SaveAsync(newMenu);

        await applicationEventPublisher.PublishAsync(new MenuCreatedEvent(command, id));

        return id;
    }
}
