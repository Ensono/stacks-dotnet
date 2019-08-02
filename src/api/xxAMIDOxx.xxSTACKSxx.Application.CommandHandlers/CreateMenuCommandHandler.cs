using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class CreateMenuCommandHandler : ICommandHandler<CreateMenu, Guid>
    {
        private IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public CreateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<Guid> HandleAsync(CreateMenu command)
        {
            var id = Guid.NewGuid();

            //TODO: Check if the user is the owner of the resource
            //if(command.User.RestaurantId != menu.RestaurantId)
            //{
            //    throw Exception
            //}


            //TODO: use a factory method for domain creation
            var newMenu = new Menu(
                id: id,
                name: command.Name,
                restaurantId: command.RestaurantId,
                description: command.Description,
                categories: null,
                enabled: command.Enabled
            );

            await repository.SaveAsync(newMenu);

            await applicationEventPublisher.PublishAsync(new MenuCreated(command, id));

            return id;
        }
    }
}
