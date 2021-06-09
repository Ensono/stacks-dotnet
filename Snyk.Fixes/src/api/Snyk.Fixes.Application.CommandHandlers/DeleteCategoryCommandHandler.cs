using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class DeleteCategoryCommandHandler : localhostCommandHandlerBase<DeleteCategory, bool>
    {
        public DeleteCategoryCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(localhost localhost, DeleteCategory command)
        {
            localhost.RemoveCategory(command.CategoryId);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, DeleteCategory command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                new CategoryDeleted(command, command.localhostId, command.CategoryId)
            };
        }
    }
}
