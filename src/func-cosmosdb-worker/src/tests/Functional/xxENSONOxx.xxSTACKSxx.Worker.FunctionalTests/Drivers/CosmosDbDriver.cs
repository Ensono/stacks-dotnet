using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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


    public async Task CreateItemAsync(string databaseName, string containerName, CosmosChangeFeedEvent item)
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


    public async Task ReplaceItemAsync(string databaseName, string containerName, CosmosChangeFeedEvent updatedItem)
    {
        try
        {
            var database = _cosmosClient.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);

            await container.ReplaceItemAsync(updatedItem, updatedItem.id, new PartitionKey(updatedItem.id));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            //return false;
        }

    }


    public async Task DeleteItemAsync(string databaseName, string containerName, CosmosChangeFeedEvent item)
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
