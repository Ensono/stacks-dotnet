using System.Collections.Generic;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

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
        return new IApplicationEvent[] {
            new MenuUpdatedEvent(command, command.MenuId)
        };
    }
}
