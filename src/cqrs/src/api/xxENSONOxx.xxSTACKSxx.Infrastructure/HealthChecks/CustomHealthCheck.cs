using System.Threading;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.Abstractions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using xxENSONOxx.xxSTACKSxx.Domain;
//using xxENSONOxx.xxSTACKSxx.Domain.Entities;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.HealthChecks;

/// <summary>
/// Implement health for a resource that does not implement IHealthCheck
/// Each resource should have it's own implementation
/// Resources implementing IHealthCheck should be added directly to the pipeline:
/// i.e: services.AddHealthChecks().AddCheck<CosmosDbDocumentStorage<Menu>>("CosmosDB");
/// </summary>
public class CustomHealthCheck(IDocumentStorage<Menu> documentStorage) : IHealthCheck
{
    //Example: do not add this example, CosmosDBStorage already implements IHealthCheck
    readonly IDocumentStorage<Menu> documentStorage = documentStorage;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        //
        //Example: do not add this example, CosmosDBStorage already implements IHealthCheck
        //var id = Guid.NewGuid().ToString();
        //var document = await this.documentStorage.GetByIdAsync(id, id);

        return await Task.FromResult(HealthCheckResult.Healthy($"{nameof(CustomHealthCheck)}: OK"));
    }
}