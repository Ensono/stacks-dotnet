using Amazon.DynamoDBv2.DocumentModel;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents;

namespace xxENSONOxx.xxSTACKSxx.Shared.DynamoDB.Abstractions;

public interface IDynamoDbObjectSearch<TEntity> where TEntity : class
{
	Task<OperationResult<List<TEntity>>> ScanAsync(ScanFilter scanFilter);
	Task<OperationResult<List<TEntity>>> QueryAsync(QueryFilter queryFilter);
}
