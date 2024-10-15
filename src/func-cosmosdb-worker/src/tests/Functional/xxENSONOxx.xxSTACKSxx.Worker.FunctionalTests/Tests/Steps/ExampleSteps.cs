using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    private readonly ServiceBusDriver _serviceBusDriver;
    private readonly string _topicName;
    private readonly string _subscriptionName;

    private IReadOnlyList<ServiceBusReceivedMessage> _activeMessageList;
    private IReadOnlyList<ServiceBusReceivedMessage> _deadLetterMessageList;
    private ServiceBusMessage _serviceBusMessage;
    private ServiceBusReceivedMessage? _serviceBusReceivedMessage;
    private ExpectedEvent _expectedCreatedEvent;
    private ExpectedEvent _expectedUpdatedEvent;

    private readonly AsyncPolicy<bool> _retryIfFalsePolicy;
    private readonly AsyncPolicy<ServiceBusReceivedMessage?> _retryIfNullPolicy;


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
        _topicName = config.TopicExample;
        _subscriptionName = config.SubsExample;

        _retryIfFalsePolicy = ServiceBusDriver.GetRetryIfFalsePolicy();
        _retryIfNullPolicy = ServiceBusDriver.GetRetryIfNullPolicy();
    }


    public async Task ClearTopicAsync()
    {
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.None);
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.DeadLetter);
    }


    public async Task DeleteExpectedItemFromCosmosDb()
    {
        await _cosmosDbDriver.DeleteItemAsync(_cosmosDatabaseName, _cosmosContainerName, _expectedCreatedEvent);
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


    public async Task ConfirmEventIsNotPresentInDeadLetter()
    {
        // Re-checks Dead letter queue for message in case of false positive
        await _retryIfFalsePolicy.ExecuteAsync(async () =>
        {
            _deadLetterMessageList = await _serviceBusDriver.ReadMessagesAsync(
                _topicName,
                _subscriptionName,
                new ServiceBusReceiverOptions
                {
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedCreatedEvent.id!))!;

            return _serviceBusReceivedMessage is not null;
        });

        // Checks for Message in DeadLetter Queue
        _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedCreatedEvent.id!))!;
        _serviceBusReceivedMessage.ShouldBeNull($"Valid message is in Deadletter queue for Id {_expectedCreatedEvent.id}");
    }


    public async Task ConfirmCreatedEventIsPresentInPendingQueue()
    {
        //Fetch the message from Pending Queue with retry
        _serviceBusReceivedMessage = await _retryIfNullPolicy.ExecuteAsync(async () =>
        {
            _activeMessageList = await _serviceBusDriver.ReadMessagesAsync(
                _topicName,
                _subscriptionName,
                new ServiceBusReceiverOptions
                {
                    SubQueue = SubQueue.None,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            // Checks for Message in Pending Queue
            var message = _activeMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedCreatedEvent.CorrelationId!));
            return message;
        });

        //Fail test if message is not found
        if (_serviceBusReceivedMessage is null)
        {
            Assert.Fail($"Message could not be found in Pending Queue after multiple retries. CorrelationId:{_expectedCreatedEvent.CorrelationId}");
        }

        //Assertion against retrieved message
        var receivedMessage = _serviceBusReceivedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<ExpectedEvent>>(receivedMessage);

        //Assert on Message
        Assert.Multiple(() =>
        {
            //actualReceivedEvent.Data.id.ShouldBe(_expectedEvent.id);
            actualReceivedEvent.Data.OperationCode.ShouldBe(_expectedCreatedEvent.OperationCode);
            actualReceivedEvent.Data.CorrelationId.ShouldBe(_expectedCreatedEvent.CorrelationId);
            actualReceivedEvent.Data.EntityId.ShouldBe(_expectedCreatedEvent.EntityId);
            actualReceivedEvent.Data.ETag.ShouldBe(_expectedCreatedEvent.ETag);
        });
    }

    public async Task ConfirmUpdateEventIsPresentInPendingQueue()
    {
        //Fetch the message from Pending Queue with retry
        _serviceBusReceivedMessage = await _retryIfNullPolicy.ExecuteAsync(async () =>
        {
            _activeMessageList = await _serviceBusDriver.ReadMessagesAsync(
                _topicName,
                _subscriptionName,
                new ServiceBusReceiverOptions
                {
                    SubQueue = SubQueue.None,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            // Checks for Message in Pending Queue
            var message = _activeMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedUpdatedEvent.CorrelationId!));
            return message;
        });

        //Fail test if message is not found
        if (_serviceBusReceivedMessage is null)
        {
            Assert.Fail($"Message could not be found in Pending Queue after multiple retries. CorrelationId:{_expectedUpdatedEvent.CorrelationId}");
        }

        //Assertion against retrieved message
        var receivedMessage = _serviceBusReceivedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<ExpectedEvent>>(receivedMessage);

        //Assert on Message
        Assert.Multiple(() =>
        {
            //actualReceivedEvent.Data.id.ShouldBe(_expectedUpdatedEvent.id);
            actualReceivedEvent.Data.OperationCode.ShouldBe(_expectedUpdatedEvent.OperationCode);
            actualReceivedEvent.Data.CorrelationId.ShouldBe(_expectedUpdatedEvent.CorrelationId);
            //actualReceivedEvent.Data.EntityId.ShouldBe(_expectedUpdatedEvent.EntityId);
            actualReceivedEvent.Data.ETag.ShouldBe(_expectedUpdatedEvent.ETag);
        });
    }


    public async Task ConfirmCosmosDbContainerExists()
    {
        var isExistingDatabase = await _cosmosDbDriver.CosmosDbExistsAsync(_cosmosDatabaseName, _cosmosContainerName);
        isExistingDatabase.ShouldBeTrue();
    }


    public async Task CreateItemInCosmosDbContainer()
    {
        var itemAsJsonString = GetDataFromFile<ExpectedEvent>("Data/Example.json");
        _expectedCreatedEvent = JsonConvert.DeserializeObject<ExpectedEvent>(itemAsJsonString)!;

        await _cosmosDbDriver.CreateItemAsync(_cosmosDatabaseName, _cosmosContainerName, _expectedCreatedEvent);
    }

    public async Task UpdateItemInCosmosDbContainer()
    {
        // Read the existing item from a JSON file
        var itemAsJsonString = GetDataFromFile<ExpectedEvent>("Data/Example.json");
        var existingItem = JsonConvert.DeserializeObject<ExpectedEvent>(itemAsJsonString)!;

        // Modify the properties of the item as needed
        existingItem.OperationCode = 98765;
        existingItem.CorrelationId = Guid.NewGuid().ToString();
        _expectedUpdatedEvent = existingItem;


        // Convert the modified item back to JSON
        var updatedItemJson = JsonConvert.SerializeObject(existingItem);

        // Get the partition key (assuming it's the ID in this case)
        var partitionKey = new PartitionKey(existingItem.id);

        // Update the item in the Cosmos DB container
        await _cosmosDbDriver.ReplaceItemAsync(_cosmosDatabaseName, _cosmosContainerName, existingItem.id, updatedItemJson, partitionKey);
    }



    private static string GetDataFromFile<T>(string dataFilePath)
    {
        var data = File.ReadAllText(dataFilePath);
        var expectedModel = JsonConvert.DeserializeObject<T>(data);
        var expected = JsonConvert.SerializeObject(expectedModel);

        return expected;
    }


    private static string GetData()
    {
        var data = File.ReadAllText("Data/Example.json");
        var expectedModel = JsonConvert.DeserializeObject<ExpectedEvent>(data);
        var expected = JsonConvert.SerializeObject(expectedModel);

        return expected;
    }


    private string GetInvalidData(string propertyToRemove)
    {
        var data = GetData();
        var jsonParsed = JObject.Parse(data);

        if (propertyToRemove.Equals("currency") || propertyToRemove.Equals("value"))
        {
            var amountObject = (JObject)jsonParsed.SelectToken("amount")!;
            amountObject.Property(propertyToRemove).Remove();
        }
        else
        {
            jsonParsed.Remove(propertyToRemove);
        }

        var jsonString = jsonParsed.ToString();

        return jsonString;
    }



}
