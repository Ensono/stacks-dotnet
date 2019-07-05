using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Events;
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

            // Apply the changes            
            await repository.Save(new Domain.Menu()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                RestaurantId = System.Guid.NewGuid(),
                Enabled = command.Enabled
            });
            //IEventPublisher.Raise.MenuCreateEvent()

            await applicationEventPublisher.PublishAsync(new MenuCreatedEvent());
        }
    }
}
