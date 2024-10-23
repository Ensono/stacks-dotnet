using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class UpdateMenuItemCommandHandler(
    IMenuRepository repository,
    IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<UpdateMenuItem, bool>(repository, applicationEventPublisher)
{
    public override Task<bool> HandleCommandAsync(Menu menu, UpdateMenuItem command)
    {
        menu.UpdateMenuItem(
            command.CategoryId,
            command.MenuItemId,
            command.Name,
            command.Description,
            command.Price,
            command.Available
        );

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, UpdateMenuItem command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId),
            //new CategoryUpdated(command, command.MenuId, command.CategoryId),
            new MenuItemUpdatedEvent(command, command.MenuId, command.CategoryId, command.MenuItemId)
        ];
    }
}
