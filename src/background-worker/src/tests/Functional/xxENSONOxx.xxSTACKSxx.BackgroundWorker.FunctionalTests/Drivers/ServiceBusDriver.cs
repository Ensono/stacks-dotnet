using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.DependencyInjection;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Drivers;

public class ServiceBusDriver
{
    private readonly ServiceBusAdministrationClient _administrationClient;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly AsyncPolicy<bool> _retryPolicy;


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
        _retryPolicy = GetTopicExistsRetryPolicy();
    }


    /// <summary>
    /// Add a new message to a topic.
    /// </summary>
    /// <param name="topicName">The name of the topic.</param>
    /// <param name="message">The message to add to the topic.</param>
    /// <returns></returns>
    public async Task AddMessageToTopic(string topicName, ServiceBusMessage message)
    {
        var sender = _serviceBusClient.CreateSender(topicName);
        try
        {
            await sender.SendMessageAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : " + e.Message);
        }
        finally
        {
            await sender.CloseAsync();
        }
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
            return await _retryPolicy.ExecuteAsync(async () =>
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
    /// Clears all messages from a topic and subscription.
    /// </summary>
    /// <param name="topicName"></param>
    /// <param name="subscriptionName"></param>
    /// <param name="subQueue"></param>
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
    public static AsyncPolicy<ServiceBusReceivedMessage?> GetMessageRetryPolicy() =>
        Policy
            .HandleResult<ServiceBusReceivedMessage?>(b => b == null)
            .WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(5));


    /// <summary>
    /// Returns a Retry Policy used when checking if a topic exists.
    /// </summary>
    public static AsyncPolicy<bool> GetTopicExistsRetryPolicy() =>
        Policy
            .HandleResult<bool>(b => !b)
            .WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(5));
}
