using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;

public class ServiceBusDriver
{
    private readonly ServiceBusAdministrationClient _administrationClient;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly AsyncPolicy<bool> _retryTopicExistsPolicy;
    private readonly AsyncPolicy<ServiceBusReceivedMessage?> _retryReceiveMessagePolicy;


    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBusDriver"/> class.
    /// </summary>
    /// <param name="clientConnectionString">The service bus connection string</param>
    /// <param name="adminConnectionString">The service bus administration client connection string</param>
    public ServiceBusDriver(string clientConnectionString, string adminConnectionString)
    {
        var serviceCollection = new ServiceCollection()
            .AddTransient(_ => new ServiceBusClient(clientConnectionString))
            .AddTransient(_ => new ServiceBusAdministrationClient(adminConnectionString));

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        _serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _administrationClient = serviceProvider.GetRequiredService<ServiceBusAdministrationClient>();
        _retryTopicExistsPolicy = GetRetryTopicExistsPolicy();
        _retryReceiveMessagePolicy = GetRetryReceiveMessageIfNullPolicy();
    }


    /// <summary>
    /// Check if given topic exits.
    /// </summary>
    /// <param name="topicPath">Name of the topic</param>
    /// <returns>A <see cref="Task"/> Returns boolean value.</returns>
    public async Task<bool> CheckTopicExistsAsync(string topicPath)
    {
        try
        {
            return await _retryTopicExistsPolicy.ExecuteAsync(async () =>
            {
                var result = await _administrationClient.TopicExistsAsync(topicPath);
                return result.HasValue && result.Value;
            });
        }
        catch (Exception ex)
        {
            return Task.FromException(ex).IsFaulted;
        }
    }


    /// <summary>
    /// Reads and returns a list of messages from a given Topic.
    /// </summary>
    /// <param name="topicName"> sets topic name</param>
    /// <param name="subscriptionName">sets subscriptions name </param>
    /// <param name="options"> sets processorOptions</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<IReadOnlyList<ServiceBusReceivedMessage>?> ReadMessagesAsync(string topicName, string subscriptionName, ServiceBusReceiverOptions options)
    {
        var receiver = _serviceBusClient.CreateReceiver(topicName, subscriptionName, options);
        try
        {
            return await receiver.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(5));
        }
        catch (AggregateException ex)
        {
            Console.WriteLine(ex.InnerException?.Message ?? "An AggregateException was thrown.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message ?? "An Exception was thrown.");
            return null;
        }
        finally
        {
            await receiver.DisposeAsync();
        }
    }


    /// <summary>
    /// Reads a given topic and subscription for a message containing the given CosmosChangeFeedEvent.
    /// </summary>
    /// <param name="topicName">The topic to search for the given message.</param>
    /// <param name="subscriptionName">The subscription to search for the given message.</param>
    /// <param name="subQueue">The sub-queue to search for the given message.</param>
    /// <param name="cosmosDbChangeFeedEvent">The message to search for</param>
    /// <returns></returns>
    public async Task<ServiceBusReceivedMessage?> ConfirmMessagePresentInQueueAsync(string topicName, string subscriptionName, SubQueue subQueue, CosmosDbChangeFeedEvent cosmosDbChangeFeedEvent)
    {
        return await _retryReceiveMessagePolicy.ExecuteAsync(async () =>
        {
            var messageList = await this.ReadMessagesAsync(
                topicName,
                subscriptionName,
                new ServiceBusReceiverOptions { SubQueue = subQueue, ReceiveMode = ServiceBusReceiveMode.PeekLock });

            var message = messageList?.FirstOrDefault(x => x.Body.ToString().Contains(cosmosDbChangeFeedEvent.CorrelationId!));
            return message;
        });
    }


    /// <summary>
    /// Clears all messages from a topic and subscription.
    /// </summary>
    /// <param name="topicName">The topic to clear.</param>
    /// <param name="subscriptionName">The subscription to clear.</param>
    /// <param name="subQueue">The sub-queue to clear.</param>
    /// <returns></returns>
    public async Task ClearTopicAsync(string topicName, string subscriptionName, SubQueue subQueue)
    {
        var receiver = _serviceBusClient.CreateReceiver(
            topicName,
            subscriptionName,
            new ServiceBusReceiverOptions
            {
                ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
                SubQueue = subQueue
            });
        while (await receiver.PeekMessageAsync() != null)
        {
            await receiver.ReceiveMessagesAsync(100);
        }

        await receiver.CloseAsync();
    }


    /// <summary>
    /// Returns a Retry Policy used when checking if a message was received.
    /// </summary>
    public static AsyncPolicy<ServiceBusReceivedMessage?> GetRetryReceiveMessageIfNullPolicy() =>
        Policy
            .HandleResult<ServiceBusReceivedMessage?>(b => b == null)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (_, timeSpan, retryCount, _) =>
                {
                    Console.WriteLine($"Retry receiving Service Bus message.  Retry count:  {retryCount}.  " +
                                      $"Waiting {timeSpan} before next retry.");
                }
            );

    /// <summary>
    /// Returns a Retry Policy used when checking if a topic exists.
    /// </summary>
    public static AsyncPolicy<bool> GetRetryTopicExistsPolicy() =>
        Policy
            .HandleResult<bool>(b => !b)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                onRetry: (_, timeSpan, retryCount, _) =>
                {
                    Console.WriteLine($"Retry checking if Topic exists.  Retry count:  {retryCount}. " +
                                      $"Waiting {timeSpan} before next retry.");
                }
            );
}
