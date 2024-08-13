using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

public class CreateMenuCommandHandler : ICommandHandler<CreateMenu, Guid>
{
    private readonly IMenuRepository repository;
    private readonly IApplicationEventPublisher applicationEventPublisher;

    public CreateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
    {
        this.repository = repository;
        this.applicationEventPublisher = applicationEventPublisher;
    }

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
