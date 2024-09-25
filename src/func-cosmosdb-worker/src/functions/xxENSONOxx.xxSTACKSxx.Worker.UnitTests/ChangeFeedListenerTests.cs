using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Worker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests;

[Trait("TestType", "UnitTests")]
public class ChangeFeedListenerUnitTests
{
    private const string DocumentsChangesMessage = "Documents modified";
    private const string DocumentReadMessage = "Document read. Id:";

    private readonly Random _random;
    private readonly IFixture _autoFixture;
    private readonly IApplicationEventPublisher _appEventPublisher;
    private readonly MockLogger<ChangeFeedListener> _logger;
    private readonly ChangeFeedListener _systemUnderTest;


    public ChangeFeedListenerUnitTests()
    {
        _random = new Random();
        _autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _appEventPublisher = Substitute.For<IApplicationEventPublisher>();
        _logger = new MockLogger<ChangeFeedListener>();
        _systemUnderTest = new ChangeFeedListener(_appEventPublisher, _logger);
    }


    [Fact]
    public void Run_ShouldPublishApplicationEvents_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEventsCount = _random.Next(1, 11);
        var changeFeedEvents = _autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        _systemUnderTest.Run(changeFeedEvents);

        // Assert
        _appEventPublisher
            .Received(changeFeedEventsCount)
            .PublishAsync(Arg.Any<IApplicationEvent>());
    }


    [Fact]
    public void Run_ShouldLogApplicationEvents_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEventsCount = _random.Next(1, 11);
        var changeFeedEvents = _autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        _systemUnderTest.Run(changeFeedEvents);

        // Assert
        _logger.LogMessages.Should().Contain($"{DocumentsChangesMessage} {changeFeedEventsCount}");

        _logger.LogMessages
            .Count(msg => msg.StartsWith(DocumentReadMessage))
            .Should().Be(changeFeedEventsCount);
    }


    [Fact]
    public void Run_ShouldPublishApplicationEventsWithExpectedValues_WhenTriggerHasChangeFeedEvents()
    {
        // Arrange
        var changeFeedEvent = _autoFixture.Create<CosmosDbChangeFeedEvent>();
        var triggerDocuments = new List<CosmosDbChangeFeedEvent> { changeFeedEvent };

        // Act
        _systemUnderTest.Run(triggerDocuments);

        // Assert
        _appEventPublisher.Received(1).PublishAsync(Arg.Is<CosmosDbChangeFeedEvent>(
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
        var changeFeedEventsCount = _autoFixture.Create<int>();
        var changeFeedEvents = _autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventsCount).ToList();

        // Act
        _systemUnderTest.Run(changeFeedEvents);

        // Assert
        foreach (var document in changeFeedEvents)
        {
            _logger.LogMessages.Should().Contain($"{DocumentReadMessage} {document.EntityId}");
        }
    }


    [Fact]
    public void Run_ShouldNotPublishApplicationEvents_WhenTriggerHasNoChangeFeedEvents()
    {
        // Act
        _systemUnderTest.Run(new List<CosmosDbChangeFeedEvent>());

        // Assert
        _appEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    [Fact]
    public void Run_ShouldNotLogApplicationEvents_WhenTriggerHasNoChangeFeedEvents()
    {
        // Act
        _systemUnderTest.Run(new List<CosmosDbChangeFeedEvent>());

        // Assert
        _logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
        _logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
    }


    [Fact]
    public void Run_ShouldNotPublishApplicationEvents_WhenTriggerChangeFeedEventsIsNull()
    {
        // Act
        _systemUnderTest.Run(null);

        // Assert
        _appEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    [Fact]
    public void Run_ShouldNotLogApplicationEvents_WhenTriggerChangeFeedEventsIsNull()
    {
        // Act
        _systemUnderTest.Run(null);

        // Assert
        _logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
        _logger.LogMessages.Should().NotContain(msg => msg.StartsWith(DocumentsChangesMessage));
    }
}
