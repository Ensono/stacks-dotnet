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
    public class DeleteMenuCommandHandler : MenuCommandHandlerBase<DeleteMenu>
    {
        public DeleteMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override async Task HandleCommandAsync(Menu menu, DeleteMenu command)
        {
            //TODO: Check if the user is the owner of the resource

            await base.repository.DeleteAsync(command.MenuId);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(Menu menu, DeleteMenu command)
        {
            return new[] { new MenuDeleted((OperationCode)command.OperationCode, command.CorrelationId, command.MenuId) };
        }

    }
}
