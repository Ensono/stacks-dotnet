using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.Common.Exceptions;
using Snyk.Fixes.CQRS.ApplicationEvents;
using Snyk.Fixes.CQRS.Commands;

namespace Snyk.Fixes.Application.CommandHandlers
{
    public class DeletelocalhostCommandHandler : ICommandHandler<Deletelocalhost, bool>
    {
        readonly IlocalhostRepository repository;
        readonly IApplicationEventPublisher applicationEventPublisher;

        public DeletelocalhostCommandHandler(IlocalhostRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<bool> HandleAsync(Deletelocalhost command)
        {
            var localhost = await repository.GetByIdAsync(command.localhostId);

            if (localhost == null)
                localhostDoesNotExistException.Raise(command, command.localhostId);

            // TODO: Check if the user owns the resource before any operation
            // if(command.User.TenantId != localhost.TenantId)
            // {
            //     throw NotAuthorizedException()
            // }

            var successful = await repository.DeleteAsync(command.localhostId);

            if (!successful) 
                OperationFailedException.Raise(command, command.localhostId, "Unable to delete localhost");

            await applicationEventPublisher.PublishAsync(
                new localhostDeleted(command, command.localhostId)
            );

            return successful;
        }
    }
}
