using xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Fixtures;

public class ChangeFeedListenerFixture
{
    private const string DocumentsChangesMessage = "Documents modified";
    private const string DocumentReadMessage = "Document read. Id:";
    
    private readonly Random random;
    private readonly IFixture autoFixture;
    private readonly IApplicationEventPublisher applicationEventPublisher;
    private readonly TestLogger<CosmosDbChangeFeedListener> logger;
    private readonly CosmosDbChangeFeedListener systemUnderTest;

    private int changeFeedEventCount;
    private List<CosmosDbChangeFeedEvent>? changeFeedEvents;

    public ChangeFeedListenerFixture()
    {
        random = new Random();
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();
        logger = new TestLogger<CosmosDbChangeFeedListener>();
        systemUnderTest = new CosmosDbChangeFeedListener(applicationEventPublisher, logger);
        changeFeedEventCount = 0;
        changeFeedEvents = [];
    }

    //
    //  Given
    //

    public void GivenCosmosDbTriggerReceivesEvents()
    {
        changeFeedEventCount = random.Next(1, 11); 
        changeFeedEvents = autoFixture.CreateMany<CosmosDbChangeFeedEvent>(changeFeedEventCount).ToList();
    }

    public void GivenCosmosDbTriggerReceivesNoEvents()
    {
        changeFeedEvents = [];
        changeFeedEventCount = changeFeedEvents.Count;
    }

    //
    //  When
    //

    public void WhenFunctionIsTriggered()
    {
        Action act = () => systemUnderTest.Run(changeFeedEvents);
        act.Should().NotThrow();
    }

    //
    // Then
    //

    public void ThenApplicationEventsArePublished()
    {
        applicationEventPublisher.Received(changeFeedEventCount).PublishAsync(Arg.Any<IApplicationEvent>());
    }


    public void ThenNoApplicationEventsArePublished()
    {
        applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CosmosDbChangeFeedEvent>());
    }


    public void ThenLogWrittenToShowEventsReceived()
    {
        logger.LogMessages.Should().Contain($"{DocumentsChangesMessage} {changeFeedEventCount}");
    }


    public void ThenNoLogWrittenToShowEventsReceived()
    {
        logger.LogMessages.Should().NotContain(DocumentsChangesMessage);
    }


    public void ThenLogWrittenWithEachEventEntityId()
    {
        foreach (var document in changeFeedEvents)
        {
            logger.LogMessages.Should().Contain($"{DocumentReadMessage} {document.EntityId}");
        }
    }


    public void ThenNoLogsWrittenWithEventEntityIds()
    {
        logger.LogMessages.Should().NotContain(DocumentReadMessage);
    }
}
