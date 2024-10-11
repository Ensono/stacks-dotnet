using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.ServiceBus;
// using Microsoft.Azure.Cosmos;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps
{
    public class CosmosDbSteps
    {
        //Replace below line with relevant event object in new project
        private ExpectedEvent _expectedEvent;


        private string item;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbSteps"/> class.
        /// </summary>
        public CosmosDbSteps()
        {
            var config = ConfigAccessor.GetApplicationConfiguration();
            //Replace _topicName with a more relavant variable name in new project
            _topicName = config.TopicExample;
            //Replace _subscriptionName with a more relavant variable name in new project
            _subscriptionName = config.SubsExample;
            var testDrivers = new TestDrivers();
            _serviceBusDriver = testDrivers.ServiceBusDriver;
            _retryPolicy = ServiceBusDriver.GetRetryPolicy();
            _serviceBusRetryPolicy = ServiceBusDriver.GetServiceBusRetryPolicy();
        }

#region Step Definitions

	#region Given

    public async Task GivenCosmosDbDocumentIsCreated()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var id = documentObject["id"].ToString();
        var partitionKey = new PartitionKey(id);

        await container.CreateItemAsync(documentObject, partitionKey);
    }

    #endregion Given

	#region When

    public async Task WhenItemIsAddedToCosmosDb(string id, 
        string operationCode, 
        string correlationId, 
        string entityId, 
        string eTag)
    // should pass the item (object?)
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var id = documentObject["id"].ToString();
        var partitionKey = new PartitionKey(id);

        await container.CreateItemAsync(documentObject, partitionKey);
    }

    public async Task WhenItemIsUpdatedInCosmosDb(string id, 
        string operationCode, 
        string correlationId, 
        string entityId, 
        string eTag)
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var id = documentObject["id"].ToString();
        var partitionKey = new PartitionKey(id);

        await container.ReplaceItemAsync(documentObject, id, partitionKey);
    }

	#endregion When

	#region Then

    // public async Task ThenConfirmEventIsPresentInPendingQueue()
    // {
    //     var config = ConfigAccessor.GetApplicationConfiguration();
    //     var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
    //     var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
    //     var container = database.GetContainer(config.CosmosDbContainerName);

    //     var documentObject = JsonConvert.DeserializeObject<JObject>(document);
    //     var id = documentObject["id"].ToString();
    //     var partitionKey = new PartitionKey(id);

    //     var response = await container.ReadItemAsync<JObject>(id, partitionKey);
    //     response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

    //     var retrievedDocument = response.Resource;
    //     retrievedDocument.ShouldNotBeNull();
    //     retrievedDocument["id"].ToString().ShouldBe(id);
    // }

    #endregion Then

	#endregion Step Definitions
        

        

        

        public string CreateCosmosDbItem(
            string id, 
            string operationCode, 
            string correlationId, 
            string entityId, 
            string eTag)
        {
            var itemObject = new
            {
                id = id,
                operationCode = operationCode,
                correlationId = correlationId,
                entityId = entityId,
                eTag = eTag
            };

            item = JsonConvert.SerializeObject(itemObject);
        }
    }
}
