using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Serializers;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Handlers;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Listeners;

public class TopicListenerTestsTests
{
    ServiceBusConfiguration config;
    IServiceCollection services;

    List<IReceiverClient> receiverClients = new();


    Handlers.TestDependency.ITestable<NotifyEvent> testable;

    public TopicListenerTestsTests()
    {
        testable = Substitute.For<Handlers.TestDependency.ITestable<NotifyEvent>>();

        config = new ServiceBusConfiguration()
        {
            Listener = new ServiceBusListenerConfiguration
            {
                Topics = new[]
                {
                    new ServiceBusSubscriptionListenerConfiguration
                    {
                        Alias = "a",
                        Name = "ALPHA"
                    },
                    new ServiceBusSubscriptionListenerConfiguration
                    {
                        Alias = "b",
                        Name = "BETA"
                    }
                }
            }
        };

        var receiverFactory = Substitute.For<IMessageReceiverClientFactory>();
        receiverFactory.CreateReceiverClient(Arg.Any<ServiceBusEntityConfiguration>()).Returns((args) =>
        {
            var cfg = (ServiceBusEntityConfiguration)args[0];
            var client = Substitute.For<FakeSBClient>();
            client.Path = cfg.Alias ?? cfg.Name;
            receiverClients.Add(client);
            return client;
        });

        services = new ServiceCollection()
                .AddLogging()
                .AddTransient((b) => Options.Create(config))
                .AddSingleton(receiverFactory)
                .AddTransient(_ => testable)
            ;
    }


    [Theory]
    [InlineData(nameof(JsonMessageSerializer), nameof(NotifyEvent))]
    [InlineData(nameof(JsonMessageSerializer), "")]
    [InlineData(nameof(CloudEventMessageSerializer), "")]
    public async Task Given_A_Message_Is_Received_From_UnknownType_Is_DeadLettered(string serializer, string enclosedTypeName)
    {
        //ARRANGE
        var guid = Guid.NewGuid();

        services
            .AddTransient<IApplicationEventHandler<NotifyEvent>, NotifyEventHandler>()
            .AddServiceBus()
            ;

        await services.BuildServiceProvider().GetService<IHostedService>().StartAsync(CancellationToken.None);

        Message msg = BuildMessage(serializer, guid);
        msg.SetLockOnMessage();
        msg.UserProperties[MessageProperties.EnclosedMessageType.ToString()] = enclosedTypeName;

        //ACT
        var client = (FakeSBClient)receiverClients.First();
        await client.SendAsyncToReceiver(msg);

        ////ASSERT
        testable.Received(0).Complete(Arg.Any<NotifyEvent>());
        await client.Received(1).DeadLetterAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(nameof(JsonMessageSerializer))]
    [InlineData(nameof(CloudEventMessageSerializer))]
    public async Task Given_A_Message_Is_Received_From_KnownType_Is_Sent_To_Right_Hanler(string serializer)
    {
        //ARRANGE
        var guid = Guid.NewGuid();

        services
            .AddTransient<IApplicationEventHandler<NotifyEvent>, NotifyEventHandler>()
            .AddServiceBus()
            ;

        await services.BuildServiceProvider().GetService<IHostedService>().StartAsync(CancellationToken.None);

        Message msg = BuildMessage(serializer, guid);
        msg.SetLockOnMessage();

        //ACT
        var client = (FakeSBClient)receiverClients.First();
        await client.SendAsyncToReceiver(msg);

        ////ASSERT
        testable.Received(1).Complete(Arg.Is<NotifyEvent>(m => m.CorrelationId == guid));
    }


    [Theory]
    [InlineData(nameof(JsonMessageSerializer))]
    [InlineData(nameof(CloudEventMessageSerializer))]
    public async Task Given_A_Message_Is_Received_From_KnownType_NoHandlerAvailable_Should_DeadLetter(string serializer)
    {
        //ARRANGE
        var guid = Guid.NewGuid();

        services.AddServiceBus();

        await services.BuildServiceProvider().GetService<IHostedService>().StartAsync(CancellationToken.None);

        Message msg = BuildMessage(serializer, guid);
        msg.SetLockOnMessage();

        //ACT
        var client = (FakeSBClient)receiverClients.First();
        await client.SendAsyncToReceiver(msg);

        ////ASSERT
        testable.Received(0).Complete(Arg.Any<NotifyEvent>());
        await client.Received(1).DeadLetterAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }


    private static Message BuildMessage(string serializer, Guid guid)
    {
        var obj = new NotifyEvent(guid, 123);

        if (serializer == nameof(CloudEventMessageSerializer))
            return new CloudEventMessageSerializer().Build<IApplicationEvent>(obj);
        else
            return new JsonMessageSerializer().Build<IApplicationEvent>(obj);
    }
}
