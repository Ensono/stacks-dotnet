using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Drivers;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Tests.Steps;

public class MenuCreatedSteps
{
    private readonly ServiceBusDriver _serviceBusDriver;
    private IReadOnlyList<ServiceBusReceivedMessage>? _activeMessageList;
    private IReadOnlyList<ServiceBusReceivedMessage>? _deadLetterMessageList;

    private readonly string _topicName;
    private readonly string _subscriptionName;
    private ServiceBusMessage _serviceBusMessage;
    private ServiceBusReceivedMessage? _serviceBusReceivedMessage;
    private StacksCloudEvent<MenuCreatedEvent> _expectedEvent;
    private readonly string _eventsNamespace;

    private readonly AsyncPolicy<bool> _retryIfFalsePolicy;
    private readonly AsyncPolicy<ServiceBusReceivedMessage?> _retryIfNullPolicy;


    /// <summary>
    /// Initializes a new instance of the <see cref="MenuCreatedSteps"/> class.
    /// </summary>
    public MenuCreatedSteps()
    {
        var config = ConfigurationAccessor.GetApplicationConfiguration();
        var testDrivers = new TestDrivers();
        _topicName = config.TopicName;
        _subscriptionName = config.SubscriptionName;
        _serviceBusDriver = testDrivers.ServiceBusDriver;
        _retryIfFalsePolicy = ServiceBusDriver.GetRetryIfFalsePolicy();
        _retryIfNullPolicy = ServiceBusDriver.GetRetryIfNullPolicy();
        _eventsNamespace = "xxENSONOxx.xxSTACKSxx.BackgroundWorker";
    }


    /// <summary>
    /// Clear all messages from a topic and subscription.
    /// </summary>
    /// <returns></returns>
    public async Task ClearTopicAsync()
    {
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.None);
        await _serviceBusDriver.ClearTopicAsync(_topicName, _subscriptionName, SubQueue.DeadLetter);
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


    /// <summary>
    /// Add a valid event to the Service Bus Topic.
    /// </summary>
    /// <returns></returns>
    public async Task AddAValidEventToServiceBus()
    {
        var eventMessageBody = GetDataFromFile<StacksCloudEvent<MenuCreatedEvent>>("Data/MenuCreatedEvent.json");
        _expectedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<MenuCreatedEvent>>(eventMessageBody)!;

        var enclosedMessageType = GetEnclosedMessageType(_expectedEvent);

        _serviceBusMessage = new ServiceBusMessage(eventMessageBody)
        {
            ContentType = "application/json",
            ApplicationProperties = { ["EnclosedMessageType"] = enclosedMessageType }
        };

        await _serviceBusDriver.AddMessageToTopic(_topicName, _serviceBusMessage);
    }


    /// <summary>
    /// Add a message with an invalid message type to the Service Bus Topic,
    /// which will prevent deserialization.
    /// </summary>
    /// <returns></returns>
    public async Task AddAInvalidEventToServiceBus()
    {
        var eventMessageBody = GetDataFromFile<StacksCloudEvent<MenuCreatedEvent>>("Data/MenuCreatedEvent.json");
        var eventMessageType = "INVALID-MESSAGE-TYPE";
        _expectedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<MenuCreatedEvent>>(eventMessageBody)!;
        _serviceBusMessage = new ServiceBusMessage(eventMessageBody)
        {
            ContentType = "application/json",
            ApplicationProperties = { ["EnclosedMessageType"] = eventMessageType }
        };
        await _serviceBusDriver.AddMessageToTopic(_topicName, _serviceBusMessage);
    }


    /// <summary>
    /// Check, with retry policy, that the worker has read and processed the message, therefore
    /// removing it from the topic.
    /// </summary>
    /// <returns></returns>
    public async Task ConfirmEventHasBeenProcessedByWorker()
    {
        //await _retryIfFalsePolicy.ExecuteAsync(async () =>
        //{
            _activeMessageList = await _serviceBusDriver.ReadMessagesAsync(
                _topicName,
                _subscriptionName,
                new ServiceBusReceiverOptions
                {
                    SubQueue = SubQueue.None,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

      //      return _activeMessageList is { Count: < 1 };
        //});

        _activeMessageList.ShouldBeEmpty("Message has not been processed by the Worker.");
    }


    /// <summary>
    /// Check that the message is not on the Dead letter queue.  A retry policy is used
    /// to ensure that the dead letter message isn't delayed to prevent a false positive.
    /// </summary>
    /// <returns></returns>
    public async Task ConfirmEventIsNotPresentInDeadLetter()
    {
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

            _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.Id))!;

            return _serviceBusReceivedMessage is not null;
        });

        _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.Id))!;
        _serviceBusReceivedMessage.ShouldBeNull($"Valid message is in the Dead Letter queue for TransactionID {_expectedEvent.Id}");
    }


    /// <summary>
    /// Fetch the message from DeadLetter with retry
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ConfirmEventIsPresentInDeadLetter()
    {
        _serviceBusReceivedMessage = await _retryIfNullPolicy.ExecuteAsync(async () =>
        {
            _deadLetterMessageList = await _serviceBusDriver.ReadMessagesAsync(
                _topicName,
                _subscriptionName,
                new ServiceBusReceiverOptions
                {
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            // Checks for Message in DeadLetter Queue
            var message = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.Id!));
            return message;
        });

        //Fail test if message is not found
        if (_serviceBusReceivedMessage is null)
        {
            Assert.Fail($"Message could not be found in DeadLetter Queue after multiple retries. TransactionID:{_expectedEvent.Id}");
        }

        var receivedMessage = _serviceBusReceivedMessage.Body.ToString();
        var actualReceivedEvent = JsonConvert.DeserializeObject<StacksCloudEvent<MenuCreatedEvent>>(receivedMessage);

        //Assert on Message
        Assert.Multiple(() =>
        {
            actualReceivedEvent.Id.ShouldBe(_expectedEvent.Id);
        });
    }


    private  string GetEnclosedMessageType<T>(StacksCloudEvent<T> message) where T : class
    {
        Type messageType = message.GetType();
        var cloudEventTypeName = messageType.FullName[..messageType.FullName.IndexOf('`')];

        var cloudEventDataType = messageType.GetGenericArguments()[0];
        var cloudEventDataTypeName = cloudEventDataType.FullName;

        return $"{cloudEventTypeName}`1[[{cloudEventDataTypeName}, {_eventsNamespace}]]";
    }


    private static string GetDataFromFile<T>(string dataFilePath)
    {
        var data = File.ReadAllText(dataFilePath);
        var expectedModel = JsonConvert.DeserializeObject<T>(data);
        var expected = JsonConvert.SerializeObject(expectedModel);

        return expected;
    }
}
