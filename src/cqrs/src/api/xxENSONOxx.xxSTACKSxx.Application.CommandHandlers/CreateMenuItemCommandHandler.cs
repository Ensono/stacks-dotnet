using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class CreateMenuItemCommandHandler(
    IMenuRepository repository,
    IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<CreateMenuItem, Guid>(repository, applicationEventPublisher)
{
    Guid id;
    public override Task<Guid> HandleCommandAsync(Menu menu, CreateMenuItem command)
    {
        id = Guid.NewGuid();

        menu.AddMenuItem(
            command.CategoryId,
            id,
            command.Name,
            command.Description,
            command.Price,
            command.Available
        );

        return Task.FromResult(id);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, CreateMenuItem command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryUpdatedEvent(command, command.MenuId, command.CategoryId),
            new MenuItemCreatedEvent(command, command.MenuId, command.CategoryId, id)
        ];
    }
}
