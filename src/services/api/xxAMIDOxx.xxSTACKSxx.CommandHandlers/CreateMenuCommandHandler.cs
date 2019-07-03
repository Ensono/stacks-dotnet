using Amido.Stacks.Application.CQRS;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;
using xxAMIDOxx.xxSTACKSxx.Models;

namespace xxAMIDOxx.xxSTACKSxx.CommandHandlers
{
    public class CreateMenuCommandHandler : ICommandHandler<CreateMenu>
    {
        private IMenuRepository repository;

        public CreateMenuCommandHandler(IMenuRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(CreateMenu command)
        {
            // TODO: Create a domain object

            // Apply the changes            
            //await repository.Save(new Domain.Menu());
        }
    }
}
