using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.ServiceBus;

public class ServiceBusDriver
{
    private readonly ManagementClient _managementClient;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly AsyncPolicy<bool> _retryPolicy;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBusDriver"/> class.
    /// </summary>
    /// <param name="connectionString">The service bus connection string</param>
    public ServiceBusDriver(string connectionString)
    {
        var serviceCollection = new ServiceCollection()
            .AddTransient(_ => new ServiceBusClient(connectionString))
            .AddTransient(_ => new ManagementClient(connectionString));

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        _managementClient = serviceProvider.GetRequiredService<ManagementClient>();
        _serviceBusClient = serviceProvider.GetRequiredService<ServiceBusClient>();
        _retryPolicy = GetRetryPolicy();
    }

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
    /// Checks if given topic exits.
    /// </summary>
    /// <param name="topicPath">Name of the topic</param>
    /// <returns>A <see cref="Task"/> Returns boolean value.</returns>
    public async Task<bool> CheckTopicExistsAsync(string topicPath)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await _managementClient.TopicExistsAsync($"/{topicPath}");
                return result;
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
    public async Task<IReadOnlyList<ServiceBusReceivedMessage>> ReadMessagesAsync(string topicName, string subscriptionName, ServiceBusReceiverOptions options)
    {
        var receiver = _serviceBusClient.CreateReceiver(topicName, subscriptionName, options);
        try
        {
            return await receiver.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(5));
        }
        catch (AggregateException ex)
        {
            Console.WriteLine(ex.InnerException.Message);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException.Message);
            return null;
        }
        finally
        {
            await receiver.DisposeAsync();
        }
    }

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
    /// Returns a Retry Policy
    /// </summary>
    public static AsyncPolicy<ServiceBusReceivedMessage?> GetServiceBusRetryPolicy() =>
        Policy
            .HandleResult<ServiceBusReceivedMessage?>(b => b == null)
            .WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(5));

    /// <summary>
    /// Returns a Retry Policy
    /// </summary>
    public static AsyncPolicy<bool> GetRetryPolicy() =>
        Policy
            .HandleResult<bool>(b => !b)
            .WaitAndRetryAsync(10, _ => TimeSpan.FromSeconds(5));
}
