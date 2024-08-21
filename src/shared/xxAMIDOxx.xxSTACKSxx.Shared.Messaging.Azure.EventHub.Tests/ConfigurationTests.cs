using xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Configuration;
using FluentAssertions;
using Xunit;
using Config = xxAMIDOxx.xxSTACKSxx.Shared.Testing.Settings.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Messaging.Azure.EventHub.Tests
{
    public class  ConfigurationTests
    {
        [Fact]
        public void Ensure_Publisher_Configuration_Is_Parsed_Correctly()
        {
            var config = Config.For<EventHubConfiguration>("EventHubConfiguration");

            config.Should().NotBeNull();
            config.Publisher.Should().NotBeNull();
            config.Publisher.NamespaceConnectionString.Should().NotBeNull();
            config.Publisher.EventHubName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Ensure_Consumer_Configuration_Is_Parsed_Correctly()
        {
            var config = Config.For<EventHubConfiguration>("EventHubConfiguration");

            config.Should().NotBeNull();
            config.Consumer.Should().NotBeNull();
            config.Consumer.NamespaceConnectionString.Should().NotBeNull();
            config.Consumer.EventHubName.Should().NotBeNullOrEmpty();
            config.Consumer.BlobStorageConnectionString.Should().NotBeNull();
            config.Consumer.BlobContainerName.Should().NotBeNullOrEmpty();
        }
    }
}
