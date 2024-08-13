using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

public class UpdateCategoryCommandHandler : MenuCommandHandlerBase<UpdateCategory, bool>
{
    public UpdateCategoryCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        : base(repository, applicationEventPublisher)
    {
    }

    public override Task<bool> HandleCommandAsync(Menu menu, UpdateCategory command)
    {
        menu.UpdateCategory(command.CategoryId, command.Name, command.Description);

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, UpdateCategory command)
    {
        return new IApplicationEvent[] {
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryUpdatedEvent(command, command.MenuId, command.CategoryId)
        };
    }
}
