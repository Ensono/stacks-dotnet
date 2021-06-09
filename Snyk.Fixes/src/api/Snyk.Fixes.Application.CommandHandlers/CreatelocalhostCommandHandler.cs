using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class CreatelocalhostCommandHandler : ICommandHandler<Createlocalhost, Guid>
    {
        private readonly IlocalhostRepository repository;
        private readonly IApplicationEventPublisher applicationEventPublisher;

        public CreatelocalhostCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<Guid> HandleAsync(Createlocalhost command)
        {
            var id = Guid.NewGuid();

            // TODO: Check if the user owns the resource before any operation
            // if(command.User.TenantId != localhost.TenantId)
            // {
            //     throw NotAuthorizedException()
            // }


            var newlocalhost = new localhost(
                id: id,
                name: command.Name,
                tenantId: command.TenantId,
                description: command.Description,
                categories: null,
                enabled: command.Enabled
            );

            await repository.SaveAsync(newlocalhost);

            await applicationEventPublisher.PublishAsync(new localhostCreated(command, id));

            return id;
        }
    }
}
