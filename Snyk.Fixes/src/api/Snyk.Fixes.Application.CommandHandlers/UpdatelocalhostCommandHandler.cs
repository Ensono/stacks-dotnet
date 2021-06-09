using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class UpdatelocalhostCommandHandler : localhostCommandHandlerBase<Updatelocalhost, bool>
    {
        public UpdatelocalhostCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        public override Task<bool> HandleCommandAsync(localhost localhost, Updatelocalhost command)
        {
            localhost.Update(command.Name, command.Description, command.Enabled);

            return Task.FromResult(true);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, Updatelocalhost command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId)
            };
        }
    }
}
