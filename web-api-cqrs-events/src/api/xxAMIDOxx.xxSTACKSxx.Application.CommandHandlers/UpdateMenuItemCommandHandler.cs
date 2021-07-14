using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class UpdateMenuItemCommandHandler : MenuCommandHandlerBase<UpdateMenuItem, bool>
    {
        public UpdateMenuItemCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

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
            return new IApplicationEvent[] {
                new MenuUpdated(command, command.MenuId),
                //new CategoryUpdated(command, command.MenuId, command.CategoryId),
                new MenuItemUpdated(command, command.MenuId, command.CategoryId, command.MenuItemId)
            };
        }
    }
}
