using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;

public class CosmosDbDriver
{
    private readonly CosmosClient _cosmosClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosDbDriver"/> class.
    /// </summary>
    /// <param name="connectionString">The service bus connection string</param>
    public CosmosDbDriver(string connectionString)
    {
        _cosmosClient = new CosmosClient(connectionString);
    }


    public async Task<bool> CosmosDbExistsAsync(string databaseName, string containerName)
    {
        try
        {
            Database cosmosDbDatabase = _cosmosClient.GetDatabase(databaseName);
            Container cosmosContainer = await cosmosDbDatabase.GetContainer(containerName).ReadContainerAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            return false;
        }
    }

    public async Task CreateItemAsync(string databaseName, string containerName, ExpectedEvent item)
    {
        try
        {
            Database database = _cosmosClient.GetDatabase(databaseName);
            Container container = await database.GetContainer(containerName).ReadContainerAsync();

            await container.CreateItemAsync(
                item: item,
                partitionKey: new PartitionKey(item.id)
            );

        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            //return false;
        }

    }

    public async Task ReplaceItemAsync(string databaseName, string containerName, string itemId, string updatedItemJson, PartitionKey partitionKey)
    {
        var database = _cosmosClient.GetDatabase(databaseName);
        var container = database.GetContainer(containerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(updatedItemJson);
        await container.ReplaceItemAsync(documentObject, itemId, partitionKey);
    }



    //public async Task WhenItemIsUpdatedInCosmosDb(string id,
    //    string operationCode,
    //    string correlationId,
    //    string entityId,
    //    string eTag)
    //{
    //    var config = ConfigAccessor.GetApplicationConfiguration();
    //    var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
    //    var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
    //    var container = database.GetContainer(config.CosmosDbContainerName);

    //    var documentObject = JsonConvert.DeserializeObject<JObject>(document);
    //    var id = documentObject["id"].ToString();
    //    var partitionKey = new PartitionKey(id);

    //    await container.ReplaceItemAsync(documentObject, id, partitionKey);
    //}

    public async Task DeleteItemAsync(string databaseName, string containerName, ExpectedEvent item)
    {
        try
        {
            Database database = _cosmosClient.GetDatabase(databaseName);
            Container container = await database.GetContainer(containerName).ReadContainerAsync();

            await container.DeleteItemAsync<dynamic>(
                item.id,
                new PartitionKey(item.id)
            );
        }
        catch (CosmosException e)
        {
            Console.WriteLine("Error : " + e.Message);
            //return false;
        }
    }
}
