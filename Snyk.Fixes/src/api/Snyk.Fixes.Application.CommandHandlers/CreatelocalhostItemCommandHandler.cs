using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class CreatelocalhostItemCommandHandler : localhostCommandHandlerBase<CreatelocalhostItem, Guid>
    {
        public CreatelocalhostItemCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        Guid id;
        public override Task<Guid> HandleCommandAsync(localhost localhost, CreatelocalhostItem command)
        {
            id = Guid.NewGuid();

            localhost.AddlocalhostItem(
                command.CategoryId,
                id,
                command.Name,
                command.Description,
                command.Price,
                command.Available
            );

            return Task.FromResult(id);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, CreatelocalhostItem command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                new CategoryUpdated(command, command.localhostId, command.CategoryId),
                new localhostItemCreated(command, command.localhostId, command.CategoryId, id)
            };
        }
    }
}
