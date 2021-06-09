using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class DeletelocalhostItemCommandHandler : localhostCommandHandlerBase<DeletelocalhostItem, bool>
    {
        public DeletelocalhostItemCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(localhost localhost, DeletelocalhostItem command)
        {
            localhost.RemovelocalhostItem(command.CategoryId, command.localhostItemId);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, DeletelocalhostItem command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                new CategoryUpdated(command, command.localhostId, command.CategoryId),
                new localhostItemDeleted(command, command.localhostId, command.CategoryId, command.localhostItemId)
            };
        }
    }
}
