using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.Commands;
using NotifyCommand = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Commands.NotifyCommand;
using NotifyEvent = xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Events.NotifyEvent;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.UnitTests.Routers;

public class FallbackMessageRouterTests
{
    ServiceBusConfiguration config;
    IServiceCollection services;

    List<ISenderClient> senderClients = new();

    public FallbackMessageRouterTests()
    {
        config = new ServiceBusConfiguration
        {
            Sender = new ServiceBusSenderConfiguration
            {
                Topics =
                [
                    new ServiceBusTopicConfiguration
                    {
                        Alias = "a",
                        Name = "ALPHA"
                    },
                    new ServiceBusTopicConfiguration
                    {
                        Alias = "b",
                        Name = "BETA"
                    }
                ],
                Queues =
                [
                    new ServiceBusQueueConfiguration
                    {
                        Alias = "c",
                        Name = "CHARLIE"
                    },
                    new ServiceBusQueueConfiguration
                    {
                        Alias = "d",
                        Name = "DELTA"
                    }
                ],
                Routing = new MessageRoutingConfiguration
                {
                    Topics =
                    [
                        new MessageRoutingTopicRouterConfiguration
                        {
                            Strategy = "fallback",
                            SendTo = ["b", "a", "b"]
                        }
                    ],
                    Queues =
                    [
                        new MessageRoutingQueueRouterConfiguration
                        {
                            SendTo = ["c", "d", "c"]
                        }
                    ],
                }
            }
        };

        var senderFactory = Substitute.For<IMessageSenderClientFactory>();
        senderFactory.CreateSenderClient(Arg.Any<ServiceBusEntityConfiguration>()).Returns((args) =>
        {
            var cfg = (ServiceBusEntityConfiguration)args[0];
            var client = Substitute.For<FakeSBClient>();
            client.Path = cfg.Alias ?? cfg.Name;
            senderClients.Add((ISenderClient)client);
            return client;
        });

        services = new ServiceCollection()
                .AddLogging()
                .AddTransient((b) => Options.Create<ServiceBusConfiguration>(config))
                .AddSingleton(senderFactory)
            ;
    }


    [Fact]
    public async Task Given_MultipleQueues_MultipleRoutesWithFilters_Commands_Are_Routed_To_Correct_Queue()
    {
        Assert.True(config.Sender.Queues.Length > 1);
        Assert.True(config.Sender.Routing.Queues.Length == 1);

        //ARRANGE
        var guid = Guid.NewGuid();
        var route = config.Sender.Routing.Queues.First();
        services.AddServiceBus();

        senderClients.Single(s => s.Path == route.SendTo.First()).SendAsync(Arg.Any<Message>()).Returns(Task.FromException(new Exception("failed")));

        //ACT
        var dispatcher = services.BuildServiceProvider().GetRequiredService<ICommandDispatcher>();
        await dispatcher.SendAsync(new NotifyCommand(guid, "123"));


        //ASSERT
        await senderClients.Single(s => s.Path == route.SendTo.First())
            .Received(1).SendAsync(Arg.Is<Message>(m => m.CorrelationId == guid.ToString()));

        await senderClients.Single(s => s.Path == route.SendTo.Skip(1).First())
            .Received(1).SendAsync(Arg.Is<Message>(m => m.CorrelationId == guid.ToString()));
    }

    [Fact]
    public async Task Given_MultipleTopics_SingleFallbackRoutes_Events_Are_SendTo_Primary_Then_Secondary()
    {
        Assert.True(config.Sender.Topics.Length > 1);
        Assert.True(config.Sender.Routing.Topics.Length == 1);

        //ARRANGE
        var guid = Guid.NewGuid();
        var route = config.Sender.Routing.Topics.First();
        services.AddServiceBus();

        senderClients.Single(s => s.Path == route.SendTo.First()).SendAsync(Arg.Any<Message>()).Returns(Task.FromException(new Exception("failed")));

        //ACT
        var publisher = services.BuildServiceProvider().GetRequiredService<IApplicationEventPublisher>();
        await publisher.PublishAsync(new NotifyEvent(guid, 1));

        //ASSERT
        await senderClients.Single(s => s.Path == route.SendTo.First())
            .Received(1).SendAsync(Arg.Is<Message>(m => m.CorrelationId == guid.ToString()));

        await senderClients.Single(s => s.Path == route.SendTo.Skip(1).First())
            .Received(1).SendAsync(Arg.Is<Message>(m => m.CorrelationId == guid.ToString()));
    }

}
