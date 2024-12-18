﻿using FluentAssertions;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.IntegrationTests.Stories
{
    public class ConfigurationTests
    {
        /// <summary>
        /// This test will ensure complex configuration setup are not breaking
        /// the serialization. e.g: using "string" instead of []{"string"}
        /// classes dependencies, and so on
        /// </summary>
        [Fact]
        public void Ensure_Configuration_Are_Parsed_Correctly()
        {
            var config = Configuration.For<ServiceBusConfiguration>("ServiceBus");
            var dispatchers = config.Sender.Routing.Topics;

            config.Should()
                .NotBeNull();

            config.Sender.Should()
                .NotBeNull();

            config.Sender.Queues.Should()
                .NotBeEmpty().And.HaveCount(1);

            config.Sender.Topics.Should()
                .NotBeEmpty().And.HaveCount(1);

            config.Sender.Routing.Should()
                .NotBeNull();

            config.Sender.Routing.Topics.Should()
                .NotBeEmpty().And.HaveCount(5);

            //TODO, check listener configuration
            //Assert.NotNull(config.Listener);
        }
    }
}
