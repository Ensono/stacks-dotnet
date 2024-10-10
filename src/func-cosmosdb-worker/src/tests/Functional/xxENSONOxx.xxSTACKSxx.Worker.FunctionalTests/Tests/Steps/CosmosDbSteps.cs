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
        private readonly ServiceBusDriver _serviceBusDriver;
        private IReadOnlyList<ServiceBusReceivedMessage> _activeMessageList;
        private readonly AsyncPolicy<bool> _retryPolicy;
        private IReadOnlyList<ServiceBusReceivedMessage> _deadLetterMessageList;
        private ServiceBusMessage _serviceBusMessage;
        private ServiceBusReceivedMessage? _serviceBusReceivedMessage;
        private readonly string _topicName;
        private readonly string _subscriptionName;
        //Replace below line with relevant event object in new project
        private ExpectedEvent _expectedEvent;
        private readonly AsyncPolicy<ServiceBusReceivedMessage?> _serviceBusRetryPolicy;

        private string document;

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

    public async Task GivenCreateCosmosDbDocument()
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

    public async Task GivenUpdateCosmosDbDocument(string id)
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var partitionKey = new PartitionKey(id);

        await container.ReplaceItemAsync(documentObject, id, partitionKey);
    }

        public async Task GivenDeleteCosmosDbDocument(string id)
        {
            var config = ConfigAccessor.GetApplicationConfiguration();
            var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
            var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
            var container = database.GetContainer(config.CosmosDbContainerName);

            var toDeletePartitionKey = new PartitionKey(id);
            await container.DeleteItemAsync<object>(id, partitionKey);
        }

    #endregion Given

	#region When

    public async Task WhenDocumentIsAddedToCosmosDb(string document)
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

    public async Task WhenDocumentIsUpdatedToCosmosDb()
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

    public async Task WhenDocumentIsDeletedFromCosmosDb()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var id = documentObject["id"].ToString();
        var partitionKey = new PartitionKey(id);

        await container.DeleteItemAsync<object>(id, partitionKey);
    }

	#endregion When

	#region Then

    public async Task ThenConfirmEventIsPresentInPendingQueue()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
        var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
        var container = database.GetContainer(config.CosmosDbContainerName);

        var documentObject = JsonConvert.DeserializeObject<JObject>(document);
        var id = documentObject["id"].ToString();
        var partitionKey = new PartitionKey(id);

        var response = await container.ReadItemAsync<JObject>(id, partitionKey);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var retrievedDocument = response.Resource;
        retrievedDocument.ShouldNotBeNull();
        retrievedDocument["id"].ToString().ShouldBe(id);
    }

    #endregion Then

	#endregion Step Definitions
        

        

        

        public string CreateCosmosDbItem(
            string id, 
            string operationCode, 
            string correlationId, 
            string entityId, 
            string eTag)
        {
            var documentObject = new
            {
                id = id,
                operationCode = operationCode,
                correlationId = correlationId,
                entityId = entityId,
                eTag = eTag
            };

            document = JsonConvert.SerializeObject(documentObject);
        }
    }
}
