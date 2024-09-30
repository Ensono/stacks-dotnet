using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps
{
    public class ServiceBusSteps
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusSteps"/> class.
        /// </summary>
        public ServiceBusSteps()
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

        public async Task ClearTopicAsync()
        {
            await _serviceBusDriver.ClearTopicAsync(
                _topicName,
                _subscriptionName,
                SubQueue.DeadLetter
                );
        }

        public async Task SubscribeToTheServiceBusTopic()
        {
            var topicExits = await _serviceBusDriver.CheckTopicExistsAsync(_topicName);
            topicExits.ShouldBe(true);
        }

        public async Task AddAValidEventToServiceBus()
        {
            var jsonMessage = GetData();

            //Replace below line with relevant event object in new project
            _expectedEvent = JsonConvert.DeserializeObject<ExpectedEvent>(jsonMessage)!;
            _serviceBusMessage = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };

            await _serviceBusDriver.AddMessageToTopic(_topicName, _serviceBusMessage);
        }

        public async Task AddAInValidEventToServiceBus(string propertyToRemove)
        {
            var jsonMessage = GetInvalidData(propertyToRemove);
            //Replace below line with relevant event object in new project
            _expectedEvent = JsonConvert.DeserializeObject<ExpectedEvent>(jsonMessage)!;
            _serviceBusMessage = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };

            await _serviceBusDriver.AddMessageToTopic(_topicName, _serviceBusMessage);
        }

        public async Task ConfirmEventIsProcessedByFunctionApp()
        {
            //Check with retry to confirm Function App has picked up the Message
            await _retryPolicy.ExecuteAsync(async () =>
            {
                _activeMessageList = await _serviceBusDriver.ReadMessagesAsync(
                   _topicName,
                   _subscriptionName,
                   new ServiceBusReceiverOptions
                   {
                       SubQueue = SubQueue.None,
                       ReceiveMode = ServiceBusReceiveMode.PeekLock
                   });

                return _activeMessageList.Count < 1;
            });

            _activeMessageList.ShouldBeEmpty("Message has not been processed by Function");
        }

        public async Task ConfirmEventIsNotPresentInDeadLetter()
        {
            //Re-checks Dead letter queue for message in case of false positive
            await _retryPolicy.ExecuteAsync(async () =>
            {
                _deadLetterMessageList = await _serviceBusDriver.ReadMessagesAsync(
                    _topicName,
                    _subscriptionName,
                    new ServiceBusReceiverOptions
                    {
                        SubQueue = SubQueue.DeadLetter,
                        ReceiveMode = ServiceBusReceiveMode.PeekLock
                    });
                //Replace below line with relevant event object in new project
                _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.TransactionId!))!;

                return _serviceBusReceivedMessage is not null;
            });
            // Checks for Message in DeadLetter Queue
            //Replace below line with relevant event object in new project
            _serviceBusReceivedMessage = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.TransactionId!))!;
            _serviceBusReceivedMessage.ShouldBeNull($"Valid message is in Deadletter queue for TransactionID {_expectedEvent.TransactionId}");
        }

        public async Task ConfirmPaymentEventIsPresentInDeadLetter()
        {
            //Fetch the message from DeadLetter with retry
            _serviceBusReceivedMessage = await _serviceBusRetryPolicy.ExecuteAsync(async () =>
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
                //Replace below line with relevant event object in new project
                var message = _deadLetterMessageList.FirstOrDefault(x => x.Body.ToString().Contains(_expectedEvent.TransactionId!));
                return message;
            });

            //Fail test if message is not found
            if (_serviceBusReceivedMessage is null)
            {
                //Replace below line with relevant event object in new project
                Assert.Fail($"Message could not be found in DeadLetter Queue after multiple retries. TransactionID:{_expectedEvent.TransactionId}");
            }

            //Assertion against retrieved message
            var receivedMessage = _serviceBusReceivedMessage.Body.ToString();
            //Replace below line with relevant event object in new project
            var actualReceivedEvent = JsonConvert.DeserializeObject<ExpectedEvent>(receivedMessage);

            //Assert on Message
            Assert.Multiple(() =>
            {
                //Replace below line with relevant event object in new project
                actualReceivedEvent.ClientId.ShouldBe(_expectedEvent.ClientId);
                //Replace below line with relevant event object in new project
                actualReceivedEvent.TransactionType.ShouldBe(_expectedEvent.TransactionType);
            });
        }

        private static string GetData()
        {
            var data = File.ReadAllText("Data/Example.json");
            //Replace below line with relevant event object in new project
            var expectedModel = JsonConvert.DeserializeObject<ExpectedEvent>(data);
            var uniqueId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
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
}
