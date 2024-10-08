using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.ServiceBus;
using Microsoft.Azure.Cosmos;

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

        public async Task CreateCosmosDbDocument()
        {
            var config = ConfigAccessor.GetApplicationConfiguration();
            var cosmosClient = new CosmosClient(config.CosmosDbConnectionString);
            var database = cosmosClient.GetDatabase(config.CosmosDbDatabaseName);
            var container = database.GetContainer(config.CosmosDbContainerName);

            await container.CreateItemAsync(document, new PartitionKey(document.id));
        }

        public async Task UpdateCosmosDbDocument()
        {
            //todo
        }

        public async Task DeleteCosmosDbDocument(string id)
        {
            //todo
        }

        public string CreateCosmosDbDocument(
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
