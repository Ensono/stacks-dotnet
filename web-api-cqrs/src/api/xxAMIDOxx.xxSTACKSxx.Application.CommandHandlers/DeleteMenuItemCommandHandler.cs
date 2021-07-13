using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class DeleteMenuItemCommandHandler : MenuCommandHandlerBase<DeleteMenuItem, bool>
    {
        public DeleteMenuItemCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(Menu menu, DeleteMenuItem command)
        {
            menu.RemoveMenuItem(command.CategoryId, command.MenuItemId);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, DeleteMenuItem command)
        {
            return new IApplicationEvent[] {
                new MenuUpdated(command, command.MenuId),
                new CategoryUpdated(command, command.MenuId, command.CategoryId),
                new MenuItemDeleted(command, command.MenuId, command.CategoryId, command.MenuItemId)
            };
        }
    }
}
