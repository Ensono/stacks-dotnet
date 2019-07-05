using Amido.Stacks.Application.CQRS;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

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

        public async Task Handle(CreateMenu command)
        {
            // TODO: Create a domain object

            // Apply the changes            
            //await repository.Save(new Domain.Menu());
            //await Task.CompletedTask;
            //IEventPublisher.Raise.MenuCreateEvent()

            await applicationEventPublisher.PublishAsync(new MenuCreatedEvent());
        }
    }
}
