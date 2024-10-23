using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class UpdateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<UpdateMenu, bool>(repository, applicationEventPublisher)
{
    public override Task<bool> HandleCommandAsync(Menu menu, UpdateMenu command)
    {
        menu.Update(command.Name, command.Description, command.Enabled);

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, UpdateMenu command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId)
        ];
    }
}
