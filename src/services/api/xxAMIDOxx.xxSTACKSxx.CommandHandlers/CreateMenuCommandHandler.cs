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
    public class CreateMenuCommandHandler : ICommandHandler<CreateMenu>
    {
        private IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public CreateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task HandleAsync(CreateMenu command)
        {
            // TODO: Create a domain object
            var menu = await repository.GetByIdAsync(command.Id);

            if (menu != null)
                MenuAlreadyExistsException.Raise(OperationId.CreateMenu, command.Id);

            // Apply the changes            
            await repository.SaveAsync(new Domain.Menu()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                RestaurantId = command.RestaurantId,
                Enabled = command.Enabled
            });

            await applicationEventPublisher.PublishAsync(new MenuCreatedEvent(command.Id));
        }
    }
}
