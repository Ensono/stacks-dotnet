using Amazon.DynamoDBv2.DocumentModel;
using xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents;

namespace xxAMIDOxx.xxSTACKSxx.Shared.DynamoDB.Abstractions;

public interface IDynamoDbObjectSearch<TEntity> where TEntity : class
{
	Task<OperationResult<List<TEntity>>> ScanAsync(ScanFilter scanFilter);
	Task<OperationResult<List<TEntity>>> QueryAsync(QueryFilter queryFilter);
}