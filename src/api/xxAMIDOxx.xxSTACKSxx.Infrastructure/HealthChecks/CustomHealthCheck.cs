using System.Threading;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.Abstractions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using xxAMIDOxx.xxSTACKSxx.Domain;
//using xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.HealthChecks
{
    /// <summary>
    /// Implement health for a resource that does not implement IHealthCheck
    /// Each resource should have it's own implementation
    /// Resources implementing IHealthCheck should be added directly to the pipeline:
    /// i.e: services.AddHealthChecks().AddCheck<CosmosDbDocumentStorage<Menu>>("CosmosDB");
    /// </summary>
    public class CustomHealthCheck : IHealthCheck
    {
        //Example: do not add this example, CosmosDBStorage already implements IHealthCheck
        readonly IDocumentStorage<Menu> documentStorage;

        public CustomHealthCheck(IDocumentStorage<Menu> documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //
            //Example: do not add this example, CosmosDBStorage already implements IHealthCheck
            //var id = Guid.NewGuid().ToString();
            //var document = await this.documentStorage.GetByIdAsync(id, id);

            return await Task.FromResult(HealthCheckResult.Healthy($"{nameof(CustomHealthCheck)}: OK"));
        }
    }
}
