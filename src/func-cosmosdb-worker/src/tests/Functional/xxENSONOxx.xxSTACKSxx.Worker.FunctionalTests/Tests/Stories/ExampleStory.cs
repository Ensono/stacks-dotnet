using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "A Stacks User",
    IWant = "to know when a change has been made in Cosmos",
    SoThat = "my downstream functions can react to the change")]

public class ExampleStory : IAsyncLifetime
{
    private readonly ExampleSteps _exampleSteps = new();


    public async Task InitializeAsync()
    {
        await _exampleSteps.ClearTopicAsync();
    }


    public async Task DisposeAsync()
    {
        await _exampleSteps.DeleteItemsCreatedInCosmosDb();
    }


    [Fact]
    public void Confirm_when_create_valid_item_in_CosmosDb_then_event_is_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.ConfirmCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.ConfirmServiceBusTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.CreateValidItemInCosmosDbContainer(), "an item with valid data is created in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageForItemCreatedIsReceived(), "a message is published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemCreatedIsNotMovedToDeadLetter(), "the message has not been moved to Dead Letters")
            .BDDfy();
    }


    [Fact]
    public void Confirm_when_attempt_to_create_invalid_item_in_CosmosDb_then_event_is_not_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.ConfirmCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.ConfirmServiceBusTopicExists(), "the Azure Service Bus Topic exists")
             .When(step => _exampleSteps.CreateInvalidItemInCosmosDbContainer(), "an item with invalid data is created in the CosmosDB container")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageForItemCreatedIsNotReceived(), "a message is not published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter(), "the message has not been moved to Dead Letters")
            .BDDfy();
    }


    [Fact]
    public void Confirm_when_update_item_in_CosmosDb_then_event_is_published_to_Service_Bus()
    {
        this.Given(step => _exampleSteps.ConfirmCosmosDbContainerExists(), "the Cosmos DB container exists")
              .And(step => _exampleSteps.ConfirmServiceBusTopicExists(), "the Azure Service Bus Topic exists")
              .And(step => _exampleSteps.CreateValidItemInCosmosDbContainer(), "an item has already been created in the container")
             .When(step => _exampleSteps.UpdateItemInCosmosDbContainer(), "the item that was created in the CosmosDB container is updated")
             .Then(step => _exampleSteps.ConfirmServiceBusMessageForItemUpdatedIsReceived(), "a message is published to the Service Bus Topic")
              .And(step => _exampleSteps.ConfirmServiceBusMessageForItemUpdatedIsNotMovedToDeadLetter(), "the message has not been moved to Dead Letters")
            .BDDfy();
    }
}
