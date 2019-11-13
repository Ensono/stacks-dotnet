using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class UpdateMenuCommandHandler : MenuCommandHandlerBase<UpdateMenu, bool>
    {
        public UpdateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(Menu menu, UpdateMenu command)
        {
            menu.Update(command.Name, command.Description, command.Enabled);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, UpdateMenu command)
        {
            return new IApplicationEvent[] {
                new MenuUpdated(command, command.MenuId)
            };
        }
    }
}
