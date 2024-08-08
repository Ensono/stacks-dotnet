using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

public class DeleteCategoryCommandHandler : MenuCommandHandlerBase<DeleteCategory, bool>
{
    public DeleteCategoryCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        : base(repository, applicationEventPublisher)
    {
    }

    public override Task<bool> HandleCommandAsync(Menu menu, DeleteCategory command)
    {
        menu.RemoveCategory(command.CategoryId);

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, DeleteCategory command)
    {
        return new IApplicationEvent[] {
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryDeletedEvent(command, command.MenuId, command.CategoryId)
        };
    }
}
