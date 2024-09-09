using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

public interface IDynamoDbObjectStorage<TEntity> where TEntity : class
{
	Task<OperationResult> DeleteAsync(string partitionKey);
	Task<OperationResult<TEntity>> GetByIdAsync(string partitionKey);
	Task<OperationResult<TEntity>> SaveAsync(string partitionKey, TEntity document);
}
