using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class DeleteCategoryCommandHandler(
    IMenuRepository repository,
    IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<DeleteCategory, bool>(repository, applicationEventPublisher)
{
    public override Task<bool> HandleCommandAsync(Menu menu, DeleteCategory command)
    {
        menu.RemoveCategory(command.CategoryId);

        return Task.FromResult(true);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, DeleteCategory command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryDeletedEvent(command, command.MenuId, command.CategoryId)
        ];
    }
}
