using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Worker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests.Tests;

[Trait("TestType", "UnitTests")]
public class ChangeFeedListenerUnitTests
{
    private const string DocumentsChangesMessage = "Documents modified:";
    private const string DocumentReadMessage = "Document read. Id:";

    private readonly Random random;
    private readonly IFixture autoFixture;
    private readonly IApplicationEventPublisher appEventPublisher;
    private readonly MockLogger<CosmosDbChangeFeedListener> logger;
    private readonly CosmosDbChangeFeedListener systemUnderTest;


    public ChangeFeedListenerUnitTests()
    {
        random = new Random();
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        appEventPublisher = Substitute.For<IApplicationEventPublisher>();
        logger = new MockLogger<CosmosDbChangeFeedListener>();
        systemUnderTest = new CosmosDbChangeFeedListener(appEventPublisher, logger);
    }


    [Fact]
    public void Run_ShouldPublishApplicationEvents_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEventsCount = random.Next(1, 11);
        var changeFeedEvents = autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        systemUnderTest.Run(changeFeedEvents);

        // Assert
        appEventPublisher
            .Received(changeFeedEventsCount)
            .PublishAsync(Arg.Any<IApplicationEvent>());
    }


    [Fact]
    public void Run_ShouldLogApplicationEvents_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEventsCount = random.Next(1, 11);
        var changeFeedEvents = autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        systemUnderTest.Run(changeFeedEvents);

        // Assert
        logger.LogMessages.Should().Contain($"{DocumentsChangesMessage} {changeFeedEventsCount}");

        logger.LogMessages
            .Count(msg => msg.StartsWith(DocumentReadMessage))
            .Should().Be(changeFeedEventsCount);
    }


    [Fact]
    public void Run_ShouldPublishApplicationEventsWithExpectedValues_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEvent = autoFixture.Create<CosmosDbChangeFeedEvent>();
        var triggerDocuments = new List<CosmosDbChangeFeedEvent> { changeFeedEvent };

        // Act
        systemUnderTest.Run(triggerDocuments);

        // Assert
        appEventPublisher.Received(1).PublishAsync(Arg.Is<CosmosDbChangeFeedEvent>(
            e => e.EventCode == changeFeedEvent.EventCode
                 && e.OperationCode == changeFeedEvent.OperationCode
                 && e.CorrelationId == changeFeedEvent.CorrelationId
                 && e.EntityId == changeFeedEvent.EntityId
                 && e.ETag == changeFeedEvent.ETag
        ));
    }


    [Fact]
    public void Run_ShouldLogApplicationEventsWithExpectedValues_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEventsCount = autoFixture.Create<int>();
        var changeFeedEvents = autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        systemUnderTest.Run(changeFeedEvents);

        // Assert
        foreach (var document in changeFeedEvents)
        {
            logger.LogMessages.Should().Contain($"{DocumentReadMessage} {document.EntityId}");
        }
    }


    [Fact]
    public void Run_ShouldNotPublishApplicationEvents_WhenTriggerHasNoChangeFeedEvents()
    {
        // Act
        systemUnderTest.Run(new List<CosmosDbChangeFeedEvent>());

        // Assert
        appEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    [Fact]
    public void Run_ShouldNotLogApplicationEvents_WhenTriggerHasNoChangeFeedEvents()
    {
        // Act
        systemUnderTest.Run(new List<CosmosDbChangeFeedEvent>());

        // Assert
        logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
        logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
    }


    [Fact]
    public void Run_ShouldNotPublishApplicationEvents_WhenTriggerChangeFeedEventsIsNull()
    {
        // Act
        systemUnderTest.Run(null);

        // Assert
        appEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    [Fact]
    public void Run_ShouldNotLogApplicationEvents_WhenTriggerChangeFeedEventsIsNull()
    {
        // Act
        systemUnderTest.Run(null);

        // Assert
        logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
        logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
    }
}
