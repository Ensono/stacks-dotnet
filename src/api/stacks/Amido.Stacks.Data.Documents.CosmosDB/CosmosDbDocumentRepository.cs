using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Exceptions;
using Microsoft.Azure.Cosmos;
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
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.PreconditionFailed:

                        DocumentHasChangedException.Raise(
                            configuration.Value.DatabaseAccountUri,
                            configuration.Value.DatabaseName,
                            container.Value.Id,
                            partitionKey,
                            identifier.ToString(),
                            eTag
                        );
                        break;
                    default:
                        DocumentUpsertException.Raise(
                            configuration.Value.DatabaseAccountUri,
                            configuration.Value.DatabaseName,
                            container.Value.Id,
                            partitionKey,
                            identifier.ToString(),
                            eTag
                        );
                        break;

                }
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
                    response.Resource,
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

                //TODO: check results if failed throw exception
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

        private ItemRequestOptions GetRequestOptions(string eTag)
        {
            return new ItemRequestOptions()
            {
                IfMatchEtag = eTag
            };
        }

        private Dictionary<string, string> GetAttributes(ItemResponse<TEntity> response)
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

        //private FeedOptions GetFeedOptions(object partitionKeyValue, int queryLimit = -1)
        //{
        //    var feedOptions = new FeedOptions
        //    {
        //        MaxItemCount = queryLimit,
        //        EnableCrossPartitionQuery = true,
        //        PartitionKey = new PartitionKey(partitionKeyValue)
        //    };
        //    return feedOptions;
        //}
    }
}
