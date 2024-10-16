using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models.CloudEvents;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;

public class ExampleSteps
{
    private readonly CosmosDbDriver _cosmosDbDriver;
    private readonly string _cosmosDatabaseName;
    private readonly string _cosmosContainerName;
    private CosmosDbChangeFeedEvent? _cosmosItemCreatedItem;
    private CosmosDbChangeFeedEvent? _cosmosItemUpdatedItem;

    private readonly ServiceBusDriver _serviceBusDriver;
    private readonly string _topicName;
    private readonly string _subscriptionName;
    private ServiceBusReceivedMessage? _serviceBusItemCreatedMessage;
    private ServiceBusReceivedMessage? _serviceBusItemUpdatedMessage;


    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleSteps"/> class.
    /// </summary>
    public ExampleSteps()
    {
        var config = ConfigurationAccessor.GetApplicationConfiguration();
        var testDrivers = new TestDrivers();

        _cosmosDbDriver = testDrivers.CosmosDbDriver;
        _cosmosDatabaseName = config.CosmosDbDatabaseName;
        _cosmosContainerName = config.CosmosDbContainerName;

        _serviceBusDriver = testDrivers.ServiceBusDriver;
        _topicName = config.TopicName;
        _subscriptionName = config.SubscriptionName;
    }


    /// <summary>
    /// Clears the Service Bus Topic, Subscription and Dead-Letters.  Run this before beginning
    /// the tests so messages from previous tests do not generate false positives.
    /// </summary>
    public async Task ClearTopicAsync()
    {
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.None);
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.DeadLetter);
    }


    /// <summary>
    /// Deletes the items that were created in Cosmos DB.  Run this after running the
    /// tests so that the items created here do not block change feed events in future tests.
    /// </summary>
    public async Task DeleteItemsCreatedInCosmosDb()
    {
        // Note that the _cosmosItemUpdatedEvent is not removed because it is the same recorded,
        // updated, so it shares an ID with the _cosmosItemCreatedEvent.
        if (_cosmosItemCreatedItem != null)
        {
            await _cosmosDbDriver.DeleteItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemCreatedItem);
        }
    }


    /// <summary>
    /// Check that a Service Bus Topic exists.
    /// </summary>
    public async Task ConfirmServiceBusTopicExists()
    {
        var topicExits = await _serviceBusDriver.CheckTopicExistsAsync(_topicName);
        topicExits.ShouldBeTrue("The Service Bus Topic does not exist.");
    }


    /// <summary>
    /// Checks that a Cosmos DB container exists.
    /// </summary>
    public async Task ConfirmCosmosDbContainerExists()
    {
        var isExistingContainer = await _cosmosDbDriver.IsExistingCosmosDbContainerAsync(_cosmosDatabaseName, _cosmosContainerName);
        isExistingContainer.ShouldBeTrue("The CosmosDB container does not exist.");
    }

    /// <summary>
    /// Create an item in the CosmosDB container with valid data.
    /// </summary>
    public async Task CreateValidItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<CosmosDbChangeFeedEvent>("Data/CosmosDbChangeFeedEvent.json");
        _cosmosItemCreatedItem = JsonConvert.DeserializeObject<CosmosDbChangeFeedEvent>(itemAsJsonString)!;

        var isCreated = await _cosmosDbDriver.CreateItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemCreatedItem);
        isCreated.ShouldBeTrue("The item could not be created in the CosmosDB container.");
    }


    /// <summary>
    /// Create an item in the CosmosDB container with invalid data.
    /// </summary>
    public async Task CreateInvalidItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<CosmosDbChangeFeedEvent>("Data/CosmosDbChangeFeedEvent-Invalid.json");
        _cosmosItemCreatedItem = JsonConvert.DeserializeObject<CosmosDbChangeFeedEvent>(itemAsJsonString)!;

        var isCreated = await _cosmosDbDriver.CreateItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemCreatedItem);
        isCreated.ShouldBeTrue("The item was created in the CosmosDB container.");
    }


    /// <summary>
    /// Update an item in the CosmosDB container.
    /// </summary>
    public async Task UpdateItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<CosmosDbChangeFeedEvent>("Data/CosmosDbChangeFeedEvent-Updated.json");
        _cosmosItemUpdatedItem = JsonConvert.DeserializeObject<CosmosDbChangeFeedEvent>(itemAsJsonString)!;

        var isUpdated = await _cosmosDbDriver.ReplaceItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemUpdatedItem);
        isUpdated.ShouldBeTrue("The item could not be created in the CosmosDB container.");
    }


    /// <summary>
    /// Read messages from the Service Bus Topic and Subscription to confirm that a message for
    /// the item created in CosmosDB was received.
    /// </summary>
    public async Task ConfirmServiceBusMessageForItemCreatedIsReceived()
    {
        _cosmosItemCreatedItem.ShouldNotBeNull();

        _serviceBusItemCreatedMessage = await _serviceBusDriver.ConfirmMessagePresentInQueueAsync(
            _topicName, _subscriptionName, SubQueue.None, _cosmosItemCreatedItem
        );

        _serviceBusItemCreatedMessage.ShouldNotBeNull(
            $"Message could not be found on {_topicName} and subscription {_subscriptionName} after multiple retries. " +
            $"CorrelationId:{_cosmosItemCreatedItem.CorrelationId}");

        var receivedMessageBody = _serviceBusItemCreatedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<CosmosDbChangeFeedEvent>>(receivedMessageBody);

        actualReceivedEvent.ShouldNotBeNull();
        actualReceivedEvent.Data.OperationCode.ShouldBe(_cosmosItemCreatedItem.OperationCode);
        actualReceivedEvent.Data.CorrelationId.ShouldBe(_cosmosItemCreatedItem.CorrelationId);
        actualReceivedEvent.Data.EntityId.ShouldBe(_cosmosItemCreatedItem.EntityId);
        actualReceivedEvent.Data.ETag.ShouldBe(_cosmosItemCreatedItem.ETag);
    }


    /// <summary>
    /// Read messages from the Service Bus Topic and Subscription to confirm that a message for
    /// the item updated in CosmosDB was received.
    /// </summary>
    public async Task ConfirmServiceBusMessageForItemUpdatedIsReceived()
    {
        _cosmosItemUpdatedItem.ShouldNotBeNull();

        _serviceBusItemUpdatedMessage = await _serviceBusDriver.ConfirmMessagePresentInQueueAsync(
            _topicName, _subscriptionName, SubQueue.None, _cosmosItemUpdatedItem
        );
        _serviceBusItemUpdatedMessage.ShouldNotBeNull(
            $"Message could not be found on {_topicName} and subscription {_subscriptionName} after multiple retries. " +
            $"CorrelationId:{_cosmosItemUpdatedItem.CorrelationId}");


        var receivedMessageBody = _serviceBusItemUpdatedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<CosmosDbChangeFeedEvent>>(receivedMessageBody);

        actualReceivedEvent.ShouldNotBeNull();
        actualReceivedEvent.Data.OperationCode.ShouldBe(_cosmosItemUpdatedItem.OperationCode);
        actualReceivedEvent.Data.CorrelationId.ShouldBe(_cosmosItemUpdatedItem.CorrelationId);
        actualReceivedEvent.Data.EntityId.ShouldBe(_cosmosItemUpdatedItem.EntityId);
        actualReceivedEvent.Data.ETag.ShouldBe(_cosmosItemUpdatedItem.ETag);
    }


    /// <summary>
    /// Read messages from the Service Bus Topic and Subscription to confirm that a message for
    /// the item created in CosmosDB was not received.
    /// </summary>
    public async Task ConfirmServiceBusMessageForItemCreatedIsNotReceived()
    {
        _cosmosItemCreatedItem.ShouldNotBeNull();

        _serviceBusItemUpdatedMessage = await _serviceBusDriver.ConfirmMessagePresentInQueueAsync(
            _topicName, _subscriptionName, SubQueue.None, _cosmosItemCreatedItem
        );

        _serviceBusItemUpdatedMessage.ShouldBeNull();
    }


    /// <summary>
    /// Read messages from the Service Bus Dead Letters to confirm that a message for
    /// the item created in CosmosDB was not Dead Lettered.
    /// </summary>
    public async Task ConfirmServiceBusMessageForItemCreatedIsNotMovedToDeadLetter()
    {
        _cosmosItemCreatedItem.ShouldNotBeNull();

        var deadLetterMessage = await _serviceBusDriver.ConfirmMessagePresentInQueueAsync(
            _topicName, _subscriptionName, SubQueue.DeadLetter, _cosmosItemCreatedItem
        );
        deadLetterMessage.ShouldBeNull();
    }


    /// <summary>
    /// Read messages from the Service Bus Dead Letters to confirm that a message for
    /// the item updated in CosmosDB was not Dead Lettered.
    /// </summary>
    public async Task ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter()
    {
        _cosmosItemUpdatedItem.ShouldNotBeNull();

        var deadLetterMessage = await _serviceBusDriver.ConfirmMessagePresentInQueueAsync(
            _topicName, _subscriptionName, SubQueue.DeadLetter, _cosmosItemUpdatedItem
        );
        deadLetterMessage.ShouldBeNull();
    }


    private static string GetDataFromFile<T>(string dataFilePath)
    {
        var data = File.ReadAllText(dataFilePath);
        var expectedModel = JsonConvert.DeserializeObject<T>(data);
        var expected = JsonConvert.SerializeObject(expectedModel);

        return expected;
    }
}
