#if (CosmosDb)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Logging;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Utilities;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;
using Container = Microsoft.Azure.Cosmos.Container;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

// but might be useful if reused with other sdks like Table Storage or MongoDB
public class CosmosDbDocumentStorage<TEntity> : IDocumentStorage<TEntity>, IDocumentSearch<TEntity>, IHealthCheck where TEntity : class
{
    ILogger<CosmosDbDocumentStorage<TEntity>> logger;

    private readonly IOptions<CosmosDbConfiguration> configuration;
    private readonly ISecretResolver<string> secretResolver;

    private string containerName;
    AsyncLazy<Container> container;
    
    public CosmosDbDocumentStorage(IOptions<CosmosDbConfiguration> configuration, ISecretResolver<string> secretResolver, ILogger<CosmosDbDocumentStorage<TEntity>> logger)
    {
        this.configuration = configuration;
        this.secretResolver = secretResolver;
        this.containerName = GetContainerName();

        container = new AsyncLazy<Container>(BuildContainer);

        this.logger = logger;// loggerFactory.CreateLogger(this.GetType());
    }

    private string GetContainerName()
    {
        TableAttribute attribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(TableAttribute));

        if (attribute == null)
        {
            return typeof(TEntity).Name;
        }
        else
        {
            return attribute.Name;
        }
    }

    /// <summary>
    /// Do not call directly. Method called by lazy loading to build the container only when used first time.
    /// </summary>
    private async Task<Container> BuildContainer()
    {
        logger.Initializing(containerName, configuration.Value.DatabaseName, configuration.Value.DatabaseAccountUri);

        var options = new CosmosClientOptions();
        options.RequestTimeout = configuration.Value.RequestTimeout;

        CosmosClient client =
            new CosmosClient(
                configuration.Value.DatabaseAccountUri,
                await secretResolver.ResolveSecretAsync(configuration.Value.SecurityKeySecret),
                options
            );

        client.ClientOptions.RequestTimeout = TimeSpan.FromSeconds(30);

        Database database = client.GetDatabase(configuration.Value.DatabaseName);
        Container container = database.GetContainer(containerName);

        return container;
    }

    public async Task<OperationResult<TEntity>> SaveAsync(string identifier, string partitionKey, TEntity document, string eTag)
    {
        if (string.IsNullOrEmpty(identifier))
            NullParameterException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                nameof(identifier)
            );

        if (document == null)
            NullParameterException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                nameof(document)
            );

        ItemResponse<TEntity> response = null;

        PartitionKey? pkey = null;
        if (!string.IsNullOrEmpty(partitionKey))
            pkey = new PartitionKey(partitionKey);

        try
        {
            logger.SaveRequested(containerName, partitionKey, identifier.ToString());

            response = await (await container).UpsertItemAsync<TEntity>(document, pkey, GetRequestOptions(eTag));

        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
        {
            logger.SaveFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            DocumentHasChangedException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                eTag
            );

        }
        catch (CosmosException ex) when (((int)ex.StatusCode) == 429)
        {
            logger.SaveFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            CosmosDBRequestCapacityExceededException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                ex
            );
        }
        catch (CosmosException ex)
        {
            logger.SaveFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            DocumentUpsertException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                eTag,
                ex
            );
        }
        catch (Exception ex)
        {
            logger.SaveFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            DocumentUpsertException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                eTag,
                ex
            );
        }

        var isSuccessfull =
            response?.StatusCode == System.Net.HttpStatusCode.OK ||
            response?.StatusCode == System.Net.HttpStatusCode.Created;

        if (isSuccessfull)
            logger.SaveCompleted(containerName, partitionKey, identifier.ToString(), response.RequestCharge);
        else
            logger.SaveFailed(containerName, partitionKey, identifier.ToString(), $"Response code is {response?.StatusCode}", null);


        return new OperationResult<TEntity>(
            isSuccessfull,
            isSuccessfull ? response.Resource : default,
            GetAttributes(response)
        );
    }

    public async Task<OperationResult<TEntity>> GetByIdAsync(string identifier, string partitionKey)
    {
        if (string.IsNullOrEmpty(identifier))
            NullParameterException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                nameof(identifier)
            );

        if (string.IsNullOrEmpty(partitionKey))
            PartitionKeyRequiredException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString()
            );

        try
        {
            logger.GetByIdRequested(containerName, partitionKey, identifier.ToString());

            var response = await (await container).ReadItemAsync<TEntity>(
                identifier.ToString(),
                new PartitionKey(partitionKey),
                GetRequestOptions(null)
            );

            var isSuccessfull = response?.StatusCode == System.Net.HttpStatusCode.OK;

            logger.GetByIdCompleted(containerName, partitionKey, identifier.ToString(), response.RequestCharge);

            return new OperationResult<TEntity>(
                isSuccessfull,
                response.Resource,
                GetAttributes(response)
            );
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.GetByIdFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            return new OperationResult<TEntity>(
                false,
                default(TEntity),
                null
            );
        }
        catch (CosmosException ex) when (((int)ex.StatusCode) == 429) //Too Many Requests
        {
            logger.GetByIdFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            CosmosDBRequestCapacityExceededException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                ex
            );
        }
        catch (Exception ex)
        {
            logger.GetByIdFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            DocumentRetrievalException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                ex
            );
        }
        return null;
    }

    public async Task<OperationResult> DeleteAsync(string identifier, string partitionKey)
    {
        if (string.IsNullOrEmpty(identifier))
            NullParameterException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                nameof(identifier)
            );

        if (string.IsNullOrEmpty(partitionKey))
            PartitionKeyRequiredException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString()
            );

        try
        {
            logger.DeleteRequested(containerName, partitionKey, identifier.ToString());

            var response = await (await container).DeleteItemAsync<TEntity>(identifier.ToString(), new PartitionKey(partitionKey), null);

            bool isSuccessful = response?.StatusCode == System.Net.HttpStatusCode.NoContent;

            if (isSuccessful)
                logger.DeleteCompleted(containerName, partitionKey, identifier.ToString(), response.RequestCharge);
            else
                logger.DeleteFailed(containerName, partitionKey, identifier.ToString(), $"Response code is {response?.StatusCode}", null);

            return new OperationResult(
                isSuccessful,
                GetAttributes(response)
            );

        }
        catch (CosmosException ex) when (((int)ex.StatusCode) == 429) //Too Many Requests
        {
            logger.DeleteFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            CosmosDBRequestCapacityExceededException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                ex
            );
        }
        catch (CosmosException ex)
        {
            logger.DeleteFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            return new OperationResult<TEntity>(
                false,
                default(TEntity),
                null
            );
        }
        catch (Exception ex)
        {
            logger.DeleteFailed(containerName, partitionKey, identifier.ToString(), ex.Message, ex);

            DocumentDeletionException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                identifier.ToString(),
                ex
            );
        }

        return null;
    }

    public async Task<OperationResult<IEnumerable<TEntity>>> Search(
        Expression<Func<TEntity, bool>> searchPredicate,
        string partitionKey = null,
        int pageSize = 20,
        int pageNumber = 1
    )
    {
        return await Search<TEntity, string>(
            searchPredicate: searchPredicate,
            orderPredicate: null,
            isAscendingOrder: true,
            partitionKey: partitionKey,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
    }

    public async Task<OperationResult<IEnumerable<TResult>>> Search<TResult>(
        Expression<Func<TResult, bool>> searchPredicate,
        string partitionKey = null,
        int pageSize = 20,
        int pageNumber = 1
    )
    {
        return await Search<TResult, string>(
            searchPredicate: searchPredicate,
            orderPredicate: null,
            isAscendingOrder: true,
            partitionKey: partitionKey,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
    }

    public async Task<OperationResult<IEnumerable<TResult>>> Search<TResult, TOrderKey>(
        Expression<Func<TResult, bool>> searchPredicate,
        Expression<Func<TResult, TOrderKey>> orderPredicate = null,
        bool isAscendingOrder = true,
        string partitionKey = null,
        int pageSize = 20,
        int pageNumber = 1
    )
    {
        if (pageNumber < 1)
            InvalidSearchParameterException.Raise(configuration.Value.DatabaseAccountUri, configuration.Value.DatabaseName, containerName, partitionKey, null, nameof(pageNumber), pageNumber.ToString(), "value > 0");

        if (searchPredicate == null)
            InvalidSearchParameterException.Raise(configuration.Value.DatabaseAccountUri, configuration.Value.DatabaseName, containerName, partitionKey, null, nameof(searchPredicate), null, "a valid search expression");

        var options = GetQueryRequestOptions(pageSize);

        if (partitionKey != null)
            options.PartitionKey = new PartitionKey(partitionKey);


        var collectionQuery =
            (await container).GetItemLinqQueryable<TResult>(
                configuration.Value.AllowSynchronousQueryExecution,
                null,
                options
            );

        if (orderPredicate != null)
        {
            if (isAscendingOrder)
                collectionQuery = collectionQuery.OrderBy(orderPredicate);
            else
                collectionQuery = collectionQuery.OrderByDescending(orderPredicate);
        }

        FeedIterator<TResult> queryResultSetIterator =
            collectionQuery
                .Where(searchPredicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToFeedIterator();

        CosmosOperationResult<IEnumerable<TResult>> result = null;
        try
        {
            logger.SearchRequested(containerName, partitionKey);
            result = result = await ParseFeedIterator<TResult>(queryResultSetIterator, true);
            logger.SearchCompleted(containerName, partitionKey, result.RequestCharge);
        }
        catch (CosmosException ex) when (((int)ex.StatusCode) == 429) //Too Many Requests
        {
            logger.SQLQueryFailed(containerName, partitionKey, ex.Message, ex);

            CosmosDBRequestCapacityExceededException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                partitionKey,
                null,
                ex
            );
        }
        catch (Exception ex)
        {
            logger.SQLQueryFailed(containerName, partitionKey, ex.Message, ex);
            throw;
        }

        return result;
    }

    public async Task<OperationResult<IEnumerable<TResult>>> RunSQLQueryAsync<TResult>(string sqlQuery, Dictionary<string, object> parameters = null, string partitionKey = null, int? MaxItemCount = null, string continuationToken = null)
    {
        var options = GetQueryRequestOptions(MaxItemCount);

        if (partitionKey != null)
            options.PartitionKey = new PartitionKey(partitionKey);

        //samples:
        //var sqlQuery = "SELECT * FROM c WHERE c.LastName = 'Andersen'";
        //var sqlQuery = "SELECT * FROM c WHERE c.LastName = @LastName"; //prefered approach

        QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                queryDefinition = queryDefinition.WithParameter($"@{param.Key}", param.Value);
            }
        }

        FeedIterator<TResult> queryResultSetIterator = (await container).GetItemQueryIterator<TResult>(queryDefinition, continuationToken, options);

        CosmosOperationResult<IEnumerable<TResult>> result;
        try
        {
            logger.SQLQueryRequested(containerName, partitionKey);
            result = await ParseFeedIterator(queryResultSetIterator);
            logger.SQLQueryCompleted(containerName, partitionKey, result.RequestCharge);
        }
        catch (Exception ex)
        {
            logger.SQLQueryFailed(containerName, partitionKey, ex.Message, ex);
            throw;
        }

        return result;
    }


    //PRIVATE METHODS

    private ItemRequestOptions GetRequestOptions(string eTag)
    {
        return new ItemRequestOptions()
        {
            IfMatchEtag = eTag
        };
    }

    private QueryRequestOptions GetQueryRequestOptions(int? MaxItemCount)
    {
        return new QueryRequestOptions()
        {
            MaxBufferedItemCount = configuration.Value.MaxBufferedItemCount,
            MaxConcurrency = configuration.Value.MaxConcurrency,
            MaxItemCount = MaxItemCount ?? configuration.Value.MaxItemCountPerBatch
        };
    }


    private Dictionary<string, string> GetAttributes(Response<TEntity> response)
    {
        var dict = new Dictionary<string, string>();

        if (response == null)
            return dict;

        dict["ETag"] = response.ETag;
        dict["RequestCharge"] = response.RequestCharge.ToString();
        dict["ActivityId"] = response.ActivityId;
        dict["MaxResourceQuota"] = response.Headers?.GetHeaderValue<string>("x-ms-resource-quota");
        dict["CurrentResourceQuotaUsage"] = response.Headers?.GetHeaderValue<string>("x-ms-resource-usage");

        return dict;
    }

    private async Task<CosmosOperationResult<IEnumerable<TResult>>> ParseFeedIterator<TResult>(FeedIterator<TResult> queryResultSetIterator, bool loadAllResults = false)
    {
        List<TResult> results = new List<TResult>();
        var dict = new Dictionary<string, string>();

        double currentCharge = 0;
        try
        {
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TResult> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (TResult item in currentResultSet)
                {
                    results.Add(item);
                }

                currentCharge += currentResultSet.RequestCharge;

                dict["RequestCharge"] = currentCharge.ToString();
                dict["ContinuationToken"] = currentResultSet.ContinuationToken;

                if (!loadAllResults)
                    break;
            }
        }
        catch (Exception ex) when (ex.Message.Contains("Resource Not Found"))
        {
            ResourceNotFoundException.Raise(
                configuration.Value.DatabaseAccountUri,
                configuration.Value.DatabaseName,
                containerName,
                null,
                containerName,
                ex
            );
        }

        return new CosmosOperationResult<IEnumerable<TResult>>(
            true,
            results,
            dict,
            currentCharge
        );
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = await (await container).ReadContainerAsync(null, cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            return await Task.FromResult(HealthCheckResult.Healthy($"{nameof(CosmosDbDocumentStorage<TEntity>)}: Ok"));
        else
            return await Task.FromResult(HealthCheckResult.Unhealthy($"{nameof(CosmosDbDocumentStorage<TEntity>)}: Failed"));
    }
}
#endif
