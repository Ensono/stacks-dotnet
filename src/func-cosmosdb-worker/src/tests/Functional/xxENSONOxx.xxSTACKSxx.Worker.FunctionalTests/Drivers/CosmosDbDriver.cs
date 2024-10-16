using Microsoft.Azure.Cosmos;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;

/// <summary>
/// Initializes a new instance of the <see cref="CosmosDbDriver"/> class.
/// </summary>
/// <param name="connectionString">The service bus connection string</param>
public class CosmosDbDriver(string connectionString)
{
    private readonly CosmosClient _cosmosClient = new(connectionString);


    /// <summary>
    /// Checks that a Cosmos DB container exists.
    /// </summary>
    /// <param name="databaseName">The name of the CosmosDB database for the container.</param>
    /// <param name="containerName">The name of the container to check if it exists.</param>
    /// <returns>Boolean value to indicate if the operation was successful.</returns>
    public async Task<bool> IsExistingCosmosDbContainerAsync(string databaseName, string containerName)
    {
        try
        {
            Database cosmosDbDatabase = _cosmosClient.GetDatabase(databaseName);
            await cosmosDbDatabase.GetContainer(containerName).ReadContainerAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error : " + ex.Message);
            return false;
        }
    }


    /// <summary>
    /// Create an item in a Cosmos DB container.
    /// </summary>
    /// <param name="databaseName">The name of the CosmosDB database for the container.</param>
    /// <param name="containerName">The name of the container to create the item in.</param>
    /// <param name="dbChangeFeedEvent">The item to create in the container.</param>
    /// <returns>Boolean value to indicate if the operation was successful.</returns>
    public async Task<bool> CreateItemAsync(string databaseName, string containerName, CosmosDbChangeFeedEvent dbChangeFeedEvent)
    {
        try
        {
            Database database = _cosmosClient.GetDatabase(databaseName);
            Container container = await database.GetContainer(containerName).ReadContainerAsync();

            await container.CreateItemAsync(
                item: dbChangeFeedEvent,
                partitionKey: new PartitionKey(dbChangeFeedEvent.id)
            );

            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            return false;
        }
    }


    /// <summary>
    /// Replace an existing item in a CosmosDB container.
    /// </summary>
    /// <param name="databaseName">The name of the CosmosDB database for the container.</param>
    /// <param name="containerName">The name of the container to replace the item in.</param>
    /// <param name="updatedDbChangeFeedEvent">The item to replace in the container, matched on ID.</param>
    /// <returns>Boolean value to indicate if the operation was successful.</returns>
    public async Task<bool> ReplaceItemAsync(string databaseName, string containerName, CosmosDbChangeFeedEvent updatedDbChangeFeedEvent)
    {
        try
        {
            var database = _cosmosClient.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);

            await container.ReplaceItemAsync(
                item: updatedDbChangeFeedEvent,
                id: updatedDbChangeFeedEvent.id,
                partitionKey: new PartitionKey(updatedDbChangeFeedEvent.id));

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
            return false;
        }
    }


    /// <summary>
    /// Replace an existing item in a CosmosDB container
    /// </summary>
    /// <param name="databaseName">The name of the CosmosDB database for the container.</param>
    /// <param name="containerName">The name of the container to delete the item from.</param>
    /// <param name="dbChangeFeedEvent">The item to delete from the container, matched on ID</param>
    /// <returns>Boolean value to indicate if the operation was successful.</returns>
    public async Task<bool> DeleteItemAsync(string databaseName, string containerName, CosmosDbChangeFeedEvent dbChangeFeedEvent)
    {
        try
        {
            Database database = _cosmosClient.GetDatabase(databaseName);
            Container container = await database.GetContainer(containerName).ReadContainerAsync();

            await container.DeleteItemAsync<dynamic>(
                dbChangeFeedEvent.id,
                new PartitionKey(dbChangeFeedEvent.id)
            );

            return true;
        }
        catch (CosmosException e)
        {
            Console.WriteLine("Error : " + e.Message);
            return false;
        }
    }
}
