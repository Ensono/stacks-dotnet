using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;

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
        return new IApplicationEvent[] {
            new MenuUpdatedEvent(command, command.MenuId),
            new CategoryCreatedEvent(command, command.MenuId, id)
        };
    }
}
