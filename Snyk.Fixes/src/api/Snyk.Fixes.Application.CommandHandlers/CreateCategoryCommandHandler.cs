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
    public class CreateCategoryCommandHandler : localhostCommandHandlerBase<CreateCategory, Guid>
    {
        public CreateCategoryCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
            : base(repository, applicationEventPublisher)
        {
        }

        Guid id;
        public override Task<Guid> HandleCommandAsync(localhost localhost, CreateCategory command)
        {
            id = Guid.NewGuid();

            localhost.AddCategory(
                id,
                command.Name,
                command.Description
            );

            return Task.FromResult(id);
        }

        public override IEnumerable<IApplicationEvent> RaiseApplicationEvents(localhost localhost, CreateCategory command)
        {
            return new IApplicationEvent[] {
                new localhostUpdated(command, command.localhostId),
                new CategoryCreated(command, command.localhostId, id)
            };
        }
    }
}
