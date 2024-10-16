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
    public void Confirm_when_create_valid_item_in_CosmosDb_then_event_is_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.CheckThatCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.CheckThatTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.CreateItemInCosmosDbContainerWithValidData(), "an item with valid data is created in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageReceivedForItemCreated(), "an event is published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemCreatedIsNotMovedToDeadLetter(), "the event does not get moved to the dead letter queue")
            .BDDfy();
    }

    [Fact]
    public void Confirm_when_attempt_to_create_invalid_item_in_CosmosDb_then_event_is_not_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.CheckThatCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.CheckThatTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.CreateItemInCosmosDbContainerWithInvalidData(), "an item with invalid data is created in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageNotReceivedForItemCreated(), "an event is published to the Service Bus Topic when the item was updated")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter(), "the event does not get moved to the dead letter queue")
            .BDDfy();
    }

    [Fact]
    public void Confirm_when_update_item_in_CosmosDb_then_event_is_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.CheckThatCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.CheckThatTopicExists(), "the Azure Service Bus Topic exists")
              .And(step => _exampleSteps.CreateItemInCosmosDbContainerWithValidData(), "an item has already been created in the container")
             .When(step => _exampleSteps.UpdateItemInCosmosDbContainer(), "the item that was created in the CosmosDB container is updated")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageReceivedForItemUpdated(), "an event is published to the Service Bus Topic when the item was updated")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter(), "the event does not get moved to the dead letter queue")
            .BDDfy();
    }
}
