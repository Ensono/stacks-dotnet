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
    private CosmosChangeFeedEvent _cosmosItemCreatedEvent;
    private CosmosChangeFeedEvent _cosmosItemUpdatedEvent;

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


    public async Task ClearTopicAsync()
    {
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.None);
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.DeadLetter);
    }


    public async Task DeleteExpectedItemFromCosmosDb()
    {
        await _cosmosDbDriver.DeleteItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemCreatedEvent);
    }


    /// <summary>
    /// Check that a Service Bus Topic exists.
    /// </summary>
    /// <returns></returns>
    public async Task CheckThatTopicExists()
    {
        var topicExits = await _serviceBusDriver.CheckTopicExistsAsync(_topicName);
        topicExits.ShouldBeTrue("The Service Bus Topic does not exist.");
    }


    public async Task CheckThatCosmosDbContainerExists()
    {
        var isExistingDatabase = await _cosmosDbDriver.CosmosDbExistsAsync(_cosmosDatabaseName, _cosmosContainerName);
        isExistingDatabase.ShouldBeTrue();
    }


    public async Task ConfirmServiceBusMessageReceivedForItemCreated()
    {
        _serviceBusItemCreatedMessage = await _serviceBusDriver.CheckEventInQueue(
            _topicName, _subscriptionName, SubQueue.None, _cosmosItemCreatedEvent
        );

        _serviceBusItemCreatedMessage.ShouldNotBeNull(
            $"Message could not be found on {_topicName} and subscription {_subscriptionName} after multiple retries. " +
            $"CorrelationId:{_cosmosItemCreatedEvent.CorrelationId}");

        var receivedMessageBody = _serviceBusItemCreatedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<CosmosChangeFeedEvent>>(receivedMessageBody);

        actualReceivedEvent.Data.OperationCode.ShouldBe(_cosmosItemCreatedEvent.OperationCode);
        actualReceivedEvent.Data.CorrelationId.ShouldBe(_cosmosItemCreatedEvent.CorrelationId);
        actualReceivedEvent.Data.EntityId.ShouldBe(_cosmosItemCreatedEvent.EntityId);
        actualReceivedEvent.Data.ETag.ShouldBe(_cosmosItemCreatedEvent.ETag);
    }


    public async Task ConfirmServiceBusMessageReceivedForItemUpdated()
    {
        _serviceBusItemUpdatedMessage = await _serviceBusDriver.CheckEventInQueue(
            _topicName, _subscriptionName, SubQueue.None, _cosmosItemUpdatedEvent
        );
        _serviceBusItemUpdatedMessage.ShouldNotBeNull(
            $"Message could not be found on {_topicName} and subscription {_subscriptionName} after multiple retries. " +
            $"CorrelationId:{_cosmosItemUpdatedEvent.CorrelationId}");


        var receivedMessageBody = _serviceBusItemUpdatedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<CosmosChangeFeedEvent>>(receivedMessageBody);

        actualReceivedEvent.Data.OperationCode.ShouldBe(_cosmosItemUpdatedEvent.OperationCode);
        actualReceivedEvent.Data.CorrelationId.ShouldBe(_cosmosItemUpdatedEvent.CorrelationId);
        actualReceivedEvent.Data.EntityId.ShouldBe(_cosmosItemUpdatedEvent.EntityId);
        actualReceivedEvent.Data.ETag.ShouldBe(_cosmosItemUpdatedEvent.ETag);
    }


    public async Task ConfirmServiceBusMessageForItemCreatedIsNotMovedToDeadLetter()
    {
        var messageFound = await _serviceBusDriver.CheckEventInQueue(
            _topicName, _subscriptionName, SubQueue.DeadLetter, _cosmosItemCreatedEvent
        );
        messageFound.ShouldBeNull("");
    }


    public async Task ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter()
    {
        var messageFound = await _serviceBusDriver.CheckEventInQueue(
            _topicName, _subscriptionName, SubQueue.DeadLetter, _cosmosItemUpdatedEvent
        );
        messageFound.ShouldBeNull("");
    }


    public async Task CreateItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<CosmosChangeFeedEvent>("Data/Example.json");
        _cosmosItemCreatedEvent = JsonConvert.DeserializeObject<CosmosChangeFeedEvent>(itemAsJsonString)!;

        await _cosmosDbDriver.CreateItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemCreatedEvent);
    }


    public async Task UpdateItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<CosmosChangeFeedEvent>("Data/ExampleUpdated.json");
        _cosmosItemUpdatedEvent = JsonConvert.DeserializeObject<CosmosChangeFeedEvent>(itemAsJsonString)!;

        await _cosmosDbDriver.ReplaceItemAsync(_cosmosDatabaseName, _cosmosContainerName, _cosmosItemUpdatedEvent);
    }


    private static string GetDataFromFile<T>(string dataFilePath)
    {
        var data = File.ReadAllText(dataFilePath);
        var expectedModel = JsonConvert.DeserializeObject<T>(data);
        var expected = JsonConvert.SerializeObject(expectedModel);

        return expected;
    }
}
