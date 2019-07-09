using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Events;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Integration;

namespace xxAMIDOxx.xxSTACKSxx.CommandHandlers
{
    public class UpdateMenuCommandHandler : ICommandHandler<UpdateMenu>
    {
        private IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public UpdateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task HandleAsync(UpdateMenu command)
        {
            var menu = await repository.GetByIdAsync(command.Id);

            if (menu == null)
                MenuDoesNotExistException.Raise(OperationId.UpdateMenu, command.Id);

            menu.Update(command.Name, command.Description, command.Enabled);

            await repository.SaveAsync(menu);

            await applicationEventPublisher.PublishAsync(new MenuUpdatedEvent(command.Id));
        }
    }
}
