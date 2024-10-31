using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers;

public class CreateCategoryCommandHandler(
    IMenuRepository repository,
    IApplicationEventPublisher applicationEventPublisher)
    : MenuCommandHandlerBase<CreateCategory, Guid>(repository, applicationEventPublisher)
{
    Guid id;
    public override Task<Guid> HandleCommandAsync(Menu menu, CreateCategory command)
    {
        id = Guid.NewGuid();

        menu.AddCategory(
            id,
            command.Name,
            command.Description
        );

        return Task.FromResult(id);
    }

    public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, CreateCategory command)
    {
        return
        [
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryCreatedEvent(command, command.MenuId, id)
        ];
    }
}
