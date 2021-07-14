using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class CreateMenuItemCommandHandler : MenuCommandHandlerBase<CreateMenuItem, Guid>
    {
        public CreateMenuItemCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

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
            return new IApplicationEvent[] {
                new MenuUpdated(command, command.MenuId),
                new CategoryUpdated(command, command.MenuId, command.CategoryId),
                new MenuItemCreated(command, command.MenuId, command.CategoryId, id)
            };
        }
    }
}
