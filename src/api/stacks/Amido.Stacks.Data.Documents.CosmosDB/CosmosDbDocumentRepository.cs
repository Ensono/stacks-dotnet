using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Data.Documents.CosmosDB
{
    //TODO: Cosmos SDK assume string for identity, we might not need the generic anymore
    public class CosmosDbDocumentRepository<TEntity, TEntityIdentityType> : IDocumentRepository<TEntity, TEntityIdentityType>
    {
        //private readonly IOptionsMonitor<CosmosDbConfiguration> configuration;
        private readonly IOptions<CosmosDbConfiguration> configuration;

        Lazy<Container> container;

        public CosmosDbDocumentRepository(IOptions<CosmosDbConfiguration> configuration)
        {
            this.configuration = configuration;

            container = new Lazy<Container>(BuildContainer);
        }

        /// <summary>
        /// Do not call directly. Method called by lazy loading to build the container only when used first time.
        /// </summary>
        private Container BuildContainer()
        {
            CosmosClient client = new CosmosClient(configuration.Value.DatabaseAccountUri, configuration.Value.SecurityKey);
            Database database = client.GetDatabase(configuration.Value.DatabaseName);
            return database.GetContainer(typeof(TEntity).Name);
        }

        public async Task<OperationResult<TEntity>> SaveAsync(TEntityIdentityType identifier, string partitionKey, TEntity document, string eTag)
        {
            ItemResponse<TEntity> response = null;

            try
            {
                response = await container.Value.UpsertItemAsync<TEntity>(document, new PartitionKey(partitionKey), GetRequestOptions(eTag));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
            {
                DocumentHasChangedException.Raise(
                    configuration.Value.DatabaseAccountUri,
                    configuration.Value.DatabaseName,
                    container.Value.Id,
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
                    container.Value.Id,
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
                    container.Value.Id,
                    partitionKey,
                    identifier.ToString(),
                    eTag,
                    ex
                );
            }


            //TODO: check results if failed throw exception
            //Maybe return the content


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
            try
            {
                var response = await container.Value.ReadItemAsync<TEntity>(identity.ToString(), new PartitionKey(partitionKey), GetRequestOptions(null));

                var isSuccessfull = response?.StatusCode == System.Net.HttpStatusCode.OK;

                return new OperationResult<TEntity>(
                        isSuccessfull,
                        response.Resource,
                        GetAttributes(response)
                    );
            }
            catch (CosmosException ex)
            {
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
                    container.Value.Id,
                    partitionKey,
                    identity.ToString(),
                    ex
                );

                return null;
            }
        }

        public async Task<OperationResult> DeleteAsync(TEntityIdentityType identity, string partitionKey)
        {
            try
            {
                var response = await container.Value.DeleteItemAsync<TEntity>(identity.ToString(), new PartitionKey(partitionKey), null);

                return new OperationResult(
                        response.StatusCode == System.Net.HttpStatusCode.NoContent,
                        GetAttributes(response)
                    );

            }
            catch (CosmosException ex)
            {
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
                    container.Value.Id,
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
                throw new Exception("Must be higher than 0");

            if (searchPredicate == null)
                throw new Exception("A search predicate is required");

            var options = GetQueryRequestOptions(pageSize);

            if (partitionKey != null)
                options.PartitionKey = new PartitionKey(partitionKey);

            var collectionQuery =
                container.Value.GetItemLinqQueryable<TResult>(
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

        public async Task<OperationResult<IEnumerable<TResult>>> RunSQLQueryAsync<TResult>(string sqlQuery, Dictionary<string, string> parameters = null, string partitionKey = null, int? MaxItemCount = null)
        {
            var options = GetQueryRequestOptions(MaxItemCount);

            if (partitionKey != null)
                options.PartitionKey = new PartitionKey(partitionKey);

            //sample:
            //var sqlQuery = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    queryDefinition = queryDefinition.WithParameter($"@{param.Key}", param.Value);
                }
            }

            FeedIterator<TResult> queryResultSetIterator = this.container.Value.GetItemQueryIterator<TResult>(queryDefinition, null, options);

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

        private async Task<OperationResult<IEnumerable<TResult>>> ParseFeedIterator<TResult>(FeedIterator<TResult> queryResultSetIterator)
        {
            List<TResult> results = new List<TResult>();
            var dict = new Dictionary<string, string>();

            double currentCharge = 0;

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TResult> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (TResult item in currentResultSet)
                {
                    results.Add(item);
                }

                currentCharge += currentResultSet.RequestCharge;

                dict["RequestCharge"] = currentCharge.ToString();
                //TODO: Consider adding continuationToken, check how to use it
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
