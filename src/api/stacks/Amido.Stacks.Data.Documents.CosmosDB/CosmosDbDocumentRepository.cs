using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Data.Documents.CosmosDB
{
    //TODO: Cosmos SDK assume string for identity, we might not need the generic anymore
    public class CosmosDbDocumentRepository<TEntity, TEntityIdentyType> : IDocumentRepository<TEntity, TEntityIdentyType>
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

        public async Task<bool> SaveAsync(TEntity document, string partitionKey, string eTag)
        {
            ItemResponse<TEntity> result = null;
            try
            {
                result = await container.Value.UpsertItemAsync<TEntity>(document, new PartitionKey(partitionKey), GetRequestOptions(eTag));
            }
            catch (Exception ex)
            {
            }

            try
            {
                result = await container.Value.UpsertItemAsync<TEntity>(document, new PartitionKey(partitionKey));
            }
            catch (Exception ex)
            {
            }

            try
            {
                result = await container.Value.UpsertItemAsync<TEntity>(document);
            }
            catch (Exception ex)
            {
            }

            //TODO: check results if failed throw exception
            //Maybe return the content
            return result?.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<TEntity> GetByIdAsync(TEntityIdentyType identity, string partitionKey)
        {
            try
            {
                var result = await container.Value.ReadItemAsync<TEntity>(identity.ToString(), new PartitionKey(partitionKey), GetRequestOptions(null));

                return result.Resource;
            }
            catch (Exception ex)
            {
                return default(TEntity);
            }
        }

        public async Task<bool> DeleteAsync(TEntityIdentyType id, string partitionKey)
        {
            var result = await container.Value.DeleteItemAsync<TEntity>(id.ToString(), new PartitionKey(partitionKey), null);

            //TODO: check results if failed throw exception
            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        private ItemRequestOptions GetRequestOptions(string eTag)
        {
            return new ItemRequestOptions()
            {
                IfMatchEtag = eTag
            };
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
