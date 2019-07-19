using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class UpdateMenuCommandHandler : MenuCommandHandlerBase<UpdateMenu>
    {
        public UpdateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task HandleCommandAsync(Menu menu, UpdateMenu command)
        {
            menu.Update(command.Name, command.Description, command.Enabled);

            return Task.CompletedTask;
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, UpdateMenu command)
        {
            return new[] { new MenuUpdated((OperationCode)command.OperationCode, command.CorrelationId, command.MenuId) };
        }
    }
}
