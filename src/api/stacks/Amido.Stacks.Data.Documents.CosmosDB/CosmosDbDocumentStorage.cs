using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amido.Stacks.Configuration;
using Amido.Stacks.Core.Utilities;
using Amido.Stacks.Data.Documents.CosmosDB.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Data.Documents.CosmosDB
{
    //TODO: Cosmos SDK assume string for identity, we might not need the generic anymore
    public class CosmosDbDocumentStorage<TEntity, TEntityIdentityType> : IDocumentStorage<TEntity, TEntityIdentityType>, IDocumentSearch<TEntity> where TEntity : class
    {
        //private readonly IOptionsMonitor<CosmosDbConfiguration> configuration;
        private readonly IOptions<CosmosDbConfiguration> configuration;
        private readonly ISecretResolver<string> secretResolver;

        private string containerName;
        AsyncLazy<Container> container;

        public CosmosDbDocumentStorage(IOptions<CosmosDbConfiguration> configuration, ISecretResolver<string> secretResolver)
        {
            this.configuration = configuration;
            this.secretResolver = secretResolver;
            this.containerName = typeof(TEntity).Name;

            container = new AsyncLazy<Container>(BuildContainer);
        }

        /// <summary>
        /// Do not call directly. Method called by lazy loading to build the container only when used first time.
        /// </summary>
        private async Task<Container> BuildContainer()
        {
            CosmosClient client =
                new CosmosClient(
                    configuration.Value.DatabaseAccountUri,
                    await secretResolver.ResolveSecretAsync(configuration.Value.SecurityKeySecret)
                );
            Database database = client.GetDatabase(configuration.Value.DatabaseName);
            return database.GetContainer(containerName);
        }

        public async Task<OperationResult<TEntity>> SaveAsync(TEntityIdentityType identifier, string partitionKey, TEntity document, string eTag)
        {
            ItemResponse<TEntity> response = null;

            PartitionKey? pkey = null;
            if (!string.IsNullOrEmpty(partitionKey))
                pkey = new PartitionKey(partitionKey);

            try
            {
                response = await (await container).UpsertItemAsync<TEntity>(document, pkey, GetRequestOptions(eTag));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
            {
                DocumentHasChangedException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    containerName,
                    partitionKey,
                    identifier.ToString(),
                    eTag
                );

            }
            catch (CosmosException ex)
            {
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

            return new OperationResult<TEntity>(
                    isSuccessfull,
                    isSuccessfull ? response.Resource : default,
                    GetAttributes(response)
                );
        }

        public async Task<OperationResult<TEntity>> GetByIdAsync(TEntityIdentityType identity, string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
                PartitionKeyRequiredException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    containerName,
                    partitionKey,
                    identity.ToString()
                );

            try
            {
                var response = await (await container).ReadItemAsync<TEntity>(
                    identity.ToString(),
                    new PartitionKey(partitionKey),
                    GetRequestOptions(null)
                );

                var isSuccessfull = response?.StatusCode == System.Net.HttpStatusCode.OK;

                return new OperationResult<TEntity>(
                        isSuccessfull,
                        response.Resource,
                        GetAttributes(response)
                    );
            }
            catch (CosmosException ex)
            {
                //TODO: log the exception

                return new OperationResult<TEntity>(
                        false,
                        default(TEntity),
                        null
                    );
            }
            catch (Exception ex)
            {
                DocumentRetrievalException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    containerName,
                    partitionKey,
                    identity.ToString(),
                    ex
                );

                return null;
            }
        }

        public async Task<OperationResult> DeleteAsync(TEntityIdentityType identity, string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
                PartitionKeyRequiredException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    containerName,
                    partitionKey,
                    identity.ToString()
                );

            try
            {
                var response = await (await container).DeleteItemAsync<TEntity>(identity.ToString(), new PartitionKey(partitionKey), null);

                return new OperationResult(
                        response.StatusCode == System.Net.HttpStatusCode.NoContent,
                        GetAttributes(response)
                    );

            }
            catch (CosmosException ex)
            {
                //TODO: log the exception

                return new OperationResult<TEntity>(
                        false,
                        default(TEntity),
                        null
                    );
            }
            catch (Exception ex)
            {
                DocumentDeletionException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    containerName,
                    partitionKey,
                    identity.ToString(),
                    ex
                );

                return null;
            }
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

            return await ParseFeedIterator<TResult>(queryResultSetIterator);
        }

        public async Task<OperationResult<IEnumerable<TResult>>> RunSQLQueryAsync<TResult>(string sqlQuery, Dictionary<string, object> parameters = null, string partitionKey = null, int? MaxItemCount = null, string continuationToken = null)
        {
            var options = GetQueryRequestOptions(MaxItemCount);

            if (partitionKey != null)
                options.PartitionKey = new PartitionKey(partitionKey);

            //samples:
            //var sqlQuery = "SELECT * FROM c WHERE c.LastName = 'Andersen'";
            //var sqlQuery = "SELECT * FROM c WHERE c.LastName = @LastName";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    queryDefinition = queryDefinition.WithParameter($"@{param.Key}", param.Value);
                }
            }

            FeedIterator<TResult> queryResultSetIterator = (await container).GetItemQueryIterator<TResult>(queryDefinition, continuationToken, options);

            return await ParseFeedIterator(queryResultSetIterator);
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
            dict["ETag"] = response.ETag;
            dict["RequestCharge"] = response.RequestCharge.ToString();
            dict["ActivityId"] = response.ActivityId;
            dict["MaxResourceQuota"] = response.Headers?.GetHeaderValue<string>("x-ms-resource-quota");
            dict["CurrentResourceQuotaUsage"] = response.Headers?.GetHeaderValue<string>("x-ms-resource-usage");

            return dict;

            //foreach (string headerName in response.Headers)
            //{
            //    var val = response.Headers?.GetHeaderValue<string>(headerName);
            //}
        }

        private async Task<OperationResult<IEnumerable<TResult>>> ParseFeedIterator<TResult>(FeedIterator<TResult> queryResultSetIterator, bool loadAllResults = false)
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

            //TODO: Calculate request charge

            return new OperationResult<IEnumerable<TResult>>(
                    true,
                    results,
                    dict
                );
        }
    }
}
