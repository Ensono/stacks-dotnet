using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class DeleteMenuItemCommandHandler(
    IMenuRepository repository,
    IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<DeleteMenuItem, bool>(repository, applicationEventPublisher)
{
    public override Task<bool> HandleCommandAsync(Menu menu, DeleteMenuItem command)
    {
        menu.RemoveMenuItem(command.CategoryId, command.MenuItemId);

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, DeleteMenuItem command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryUpdatedEvent(command, command.MenuId, command.CategoryId),
            new MenuItemDeletedEvent(command, command.MenuId, command.CategoryId, command.MenuItemId)
        ];
    }
}
