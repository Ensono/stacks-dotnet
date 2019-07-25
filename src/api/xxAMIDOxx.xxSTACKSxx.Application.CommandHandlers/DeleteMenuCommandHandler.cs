using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class DeleteMenuCommandHandler : ICommandHandler<DeleteMenu>
    {
        IMenuRepository repository;
        IApplicationEventPublisher applicationEventPublisher;

        public DeleteMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task HandleAsync(DeleteMenu command)
        {
            var menu = repository.GetByIdAsync(command.MenuId);

            //TODO: Check if the user is the owner of the resource
            //if(command.User.RestaurantId != menu.RestaurantId)
            //{
            //    throw Exception
            //}

            await repository.DeleteAsync(command.MenuId);

            await applicationEventPublisher.PublishAsync(
                new MenuDeleted(command, command.MenuId)
            );
        }
    }
}
