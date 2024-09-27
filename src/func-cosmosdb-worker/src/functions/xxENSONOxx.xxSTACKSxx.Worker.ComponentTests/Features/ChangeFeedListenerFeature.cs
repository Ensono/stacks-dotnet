using xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Fixtures;

namespace xxENSONOxx.xxSTACKSxx.Worker.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class ChangeFeedListenerFeature
{
    [Theory, AutoData]
    public void ApplicationEventsAreRaisedWhenChangeFeedEventsAreReceived(ChangeFeedListenerFixture fixture)
    {
        this.Given(_ => fixture.GivenCosmosDbTriggerReceivesEvents(), "Given the CosmosDBTrigger receives change feed events")
             .When(_ => fixture.WhenFunctionIsTriggered(), "When the Azure Function is triggered")
             .Then(_ => fixture.ThenApplicationEventsArePublished(), "Then an application event is published for each change feed event")
              .And(_ => fixture.ThenLogWrittenToShowEventsReceived(), "and an information log is written to that show change feed events have been received")
              .And(_ => fixture.ThenLogWrittenWithEachEventEntityId(), "and an information log is written showing the document's Entity ID")
            .BDDfy();
    }


    [Theory, AutoData]
    public void NoApplicationEventsAreRaisedWhenChangeFeedEventsAreNotReceived(ChangeFeedListenerFixture fixture)
    {
        this.Given(_ => fixture.GivenCosmosDbTriggerReceivesNoEvents(), "Given the CosmosDBTrigger does not receive change feed events")
             .When(_ => fixture.WhenFunctionIsTriggered(), "When the Azure Function is triggered")
             .Then(_ => fixture.ThenNoApplicationEventsArePublished(), "Then no application events are published")
              .And(_ => fixture.ThenNoLogWrittenToShowEventsReceived(), "and no information log is written to that show change feed events have been received")
              .And(_ => fixture.ThenNoLogsWrittenWithEventEntityIds(), "and no logs are written showing document's ID")
            .BDDfy();
    }
}
