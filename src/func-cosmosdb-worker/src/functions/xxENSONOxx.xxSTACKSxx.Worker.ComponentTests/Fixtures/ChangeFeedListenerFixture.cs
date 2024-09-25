using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Fixtures;

public class ChangeFeedListenerFixture
{
    private const string DocumentsChangesMessage = "Documents modified";
    private const string DocumentReadMessage = "Document read. Id:";
    
    private readonly Random _random;
    private readonly IFixture _autoFixture;
    private readonly IApplicationEventPublisher _applicationEventPublisher;
    private readonly TestLogger<CosmosDbChangeFeedListener> _logger;
    private readonly CosmosDbChangeFeedListener _systemUnderTest;

    private int _changeFeedEventCount;
    private List<CosmosDbChangeFeedEvent>? _changeFeedEvents;

    public ChangeFeedListenerFixture()
    {
        _random = new Random();
        _autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();
        _logger = new TestLogger<CosmosDbChangeFeedListener>();
        _systemUnderTest = new CosmosDbChangeFeedListener(_applicationEventPublisher, _logger);
        _changeFeedEventCount = 0;
        _changeFeedEvents = [];
    }

    //
    //  Given
    //

    public void GivenCosmosDbTriggerReceivesEvents()
    {
        _changeFeedEventCount = _random.Next(1, 11); 
        _changeFeedEvents = _autoFixture.CreateMany<CosmosDbChangeFeedEvent>(_changeFeedEventCount).ToList();
    }

    public void GivenCosmosDbTriggerReceivesNoEvents()
    {
        _changeFeedEvents = [];
        _changeFeedEventCount = _changeFeedEvents.Count;
    }

    //
    //  When
    //

    public void WhenFunctionIsTriggered()
    {
        Action act = () => _systemUnderTest.Run(_changeFeedEvents);
        act.Should().NotThrow();
    }

    //
    // Then
    //

    public void ThenApplicationEventsArePublished()
    {
        _applicationEventPublisher.Received(_changeFeedEventCount).PublishAsync(Arg.Any<IApplicationEvent>());
    }


    public void ThenNoApplicationEventsArePublished()
    {
        _applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    public void ThenLogWrittenToShowEventsReceived()
    {
        _logger.LogMessages.Should().Contain($"{DocumentsChangesMessage} {_changeFeedEventCount}");
    }


    public void ThenNoLogWrittenToShowEventsReceived()
    {
        _logger.LogMessages.Should().NotContain(DocumentsChangesMessage);
    }


    public void ThenLogWrittenWithEachEventEntityId()
    {
        foreach (var document in _changeFeedEvents)
        {
            _logger.LogMessages.Should().Contain($"{DocumentReadMessage} {document.EntityId}");
        }
    }


    public void ThenNoLogsWrittenWithEventEntityIds()
    {
        _logger.LogMessages.Should().NotContain(DocumentReadMessage);
    }
}
