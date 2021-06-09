using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class UpdateCategoryCommandHandler : localhostCommandHandlerBase<UpdateCategory, bool>
    {
        public UpdateCategoryCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(localhost localhost, UpdateCategory command)
        {
            localhost.UpdateCategory(command.CategoryId, command.Name, command.Description);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, UpdateCategory command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                new CategoryUpdated(command, command.localhostId, command.CategoryId)
            };
        }
    }
}
