using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class UpdatelocalhostItemCommandHandler : localhostCommandHandlerBase<UpdatelocalhostItem, bool>
    {
        public UpdatelocalhostItemCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(localhost localhost, UpdatelocalhostItem command)
        {
            localhost.UpdatelocalhostItem(
                command.CategoryId,
                command.localhostItemId,
                command.Name,
                command.Description,
                command.Price,
                command.Available
            );

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, UpdatelocalhostItem command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                //new CategoryUpdated(command, command.localhostId, command.CategoryId),
                new localhostItemUpdated(command, command.localhostId, command.CategoryId, command.localhostItemId)
            };
        }
    }
}
