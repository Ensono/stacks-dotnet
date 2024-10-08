#if (DynamoDb)
using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Logging;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

public class DynamoDbObjectStorage<TEntity>(
    ILogger<DynamoDbObjectStorage<TEntity>> logger,
    IDynamoDBContext context,
    IOptions<DynamoDbConfiguration> config)
    : IDynamoDbObjectStorage<TEntity>
    where TEntity : class
{
	private ILogger<DynamoDbObjectStorage<TEntity>> logger = logger ?? throw new ArgumentNullException(nameof(logger));
	private readonly IDynamoDBContext context = context ?? throw new ArgumentNullException(nameof(context));
	private readonly IOptions<DynamoDbConfiguration> config = config ?? throw new ArgumentNullException(nameof(config));
	private readonly DynamoDBOperationConfig opearationConfig = new()
    {
        OverrideTableName = config.Value.TableName,
        TableNamePrefix = config.Value.TablePrefix
    };

    public async Task<OperationResult> DeleteAsync(string partitionKey)
	{
		try
		{
			logger.DeleteRequested(partitionKey);

			await context.DeleteAsync<TEntity>(partitionKey, opearationConfig);

			logger.DeleteCompleted(partitionKey);

			return new OperationResult(true, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.DeleteFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.DeleteFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}

	public async Task<OperationResult<TEntity>> GetByIdAsync(string partitionKey)
	{
		try
		{
			logger.GetByIdRequested(partitionKey);

			var result = await context.LoadAsync<TEntity>(partitionKey, opearationConfig);

			logger.GetByIdCompleted(partitionKey);

			return new OperationResult<TEntity>(true, result, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.GetByIdFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.GetByIdFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}

	public async Task<OperationResult<TEntity>> SaveAsync(string partitionKey, TEntity document)
	{
		try
		{
			logger.SaveRequested(partitionKey);

			await context.SaveAsync(document, opearationConfig);

			logger.SaveCompleted(partitionKey);

			return new OperationResult<TEntity>(true, document, null);
		}
		catch (AmazonServiceException ex)
		{
			logger.SaveFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
		catch (AmazonClientException ex)
		{
			logger.SaveFailed(partitionKey, ex.Message, ex);

			return new OperationResult<TEntity>(
				false,
				default(TEntity)!,
				null);
		}
	}
}
#endif
