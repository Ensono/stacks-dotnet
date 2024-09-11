using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

public interface IDynamoDbObjectSearch<TEntity> where TEntity : class
{
	Task<OperationResult<List<TEntity>>> ScanAsync(ScanFilter scanFilter);
	Task<OperationResult<List<TEntity>>> QueryAsync(QueryFilter queryFilter);
}
