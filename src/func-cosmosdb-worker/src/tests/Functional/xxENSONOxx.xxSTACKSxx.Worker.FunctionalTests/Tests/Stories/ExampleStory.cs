using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "A Stacks User",
    IWant = "to know when a change has been made in Cosmos",
    SoThat = "my downstream functions can react to the change")]

//TODO check this line
public class ExampleStory : IAsyncLifetime
{
    private readonly ExampleSteps _exampleSteps = new();

    public async Task InitializeAsync()
    {
        await _exampleSteps.ClearTopicAsync();
    }

    public async Task DisposeAsync()
    {
        await _exampleSteps.DeleteExpectedItemFromCosmosDb();
    }

    [Fact]
    public void Confirm_event_is_published_to_service_bus_when_item_is_created_in_cosmosdb()
    {
        this.Given(step => _exampleSteps.ConfirmCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.CheckThatTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.CreateItemInCosmosDbContainer(), "an item is created in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmCreatedEventIsPresentInPendingQueue(), "an event is published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmEventIsNotPresentInDeadLetter(), "the event does not get sent to the dead letter queue")
            .BDDfy();

    }

    [Fact]
    public void Confirm_event_is_published_to_service_bus_when_item_is_updated_in_cosmosdb()
    {
        this.Given(step => _exampleSteps.ConfirmCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.CreateItemInCosmosDbContainer(), "an item has been created in the container")
              .And(step => _exampleSteps.CheckThatTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.UpdateItemInCosmosDbContainer(), "the item is updated in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmCreatedEventIsPresentInPendingQueue(), "a created event is published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmUpdateEventIsPresentInPendingQueue(), "an updated event is published to the Service Bus Topic")
            // .And(step => _exampleSteps.ConfirmEventIsNotPresentInDeadLetter(), "the event does not get sent to the dead letter queue")
            .BDDfy();
    }

    ////TODO: check how to complete the test below, since all the changes in DB go to DeadLetter
    //public void Confirm_invalid_event_is_not_received_by_function_app(string invalidProperty)
    //{
    //    this.Given(s => _exampleSteps.SubscribeToTheServiceBusTopic())
    //        .When(s => _exampleSteps.AddAInValidEventToServiceBus(invalidProperty))
    //        .Then(s => _exampleSteps.ConfirmEventIsPresentInDeadLetter())
    //        .BDDfy();
    //}
}
