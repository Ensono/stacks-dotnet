using System;
using System.Threading;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Senders;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.IntegrationTests.Steps
{
    public class SendingCommandFixtures
    {
        private readonly ServiceProvider _provider;
        private readonly ITestable<NotifyCommand> _testable;
        private Guid _correlationId;

        public SendingCommandFixtures()
        {
            _testable = Substitute.For<ITestable<NotifyCommand>>();

            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.integration.queues.json")
                .Build();

            var services = new ServiceCollection()
                .AddLogging()
                .AddSecrets()
                .AddTransient<ICommandHandler<NotifyCommand, bool>, NotifyCommandHandler>()
                .AddTransient(_ => _testable)
                .Configure<ServiceBusConfiguration>(configurationRoot.GetSection("ServiceBus"))
                .AddServiceBus()
            ;

            _provider = services.BuildServiceProvider();
            _correlationId = Guid.NewGuid();
        }

        public async Task TheQueueSenderHealthCheckPass()
        {
            var queues = _provider.GetServices<IQueueSender>();
            foreach (var queue in queues)
            {
                var check = await ((IHealthCheck)queue).CheckHealthAsync(null);
                check.Status.Should().Be(HealthStatus.Healthy);
            }
        }

        public async Task TheCorrectCommandIsSentToTheQueue()
        {
            var dispatcher = _provider.GetService<ICommandDispatcher>();
            await dispatcher.SendAsync(new NotifyCommand(_correlationId, "MessageInTheBottle"));
        }

        public void TheMessageIsHandledInTheHandler()
        {
            _testable.Received(1)
                .Complete(Arg.Is<NotifyCommand>(command => command.TestMember == "MessageInTheBottle" && command.CorrelationId == _correlationId));
        }

        public void TheHostIsRunning()
        {
            var hostedService = _provider.GetService<IHostedService>();
            hostedService.StartAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public void WaitFor3Seconds()
        {
            Task.Delay(TimeSpan.FromSeconds(3))
                .GetAwaiter().GetResult();
        }
    }
}