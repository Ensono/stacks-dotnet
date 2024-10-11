using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;


namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "A Stacks User",
    IWant = "to know when a change has been made in Cosmos",
    SoThat = "my downstream functions can react to the change")]

//TODO check this line
public class ExpectedEvent : IAsyncLifetime
{
    private readonly ServiceBusSteps serviceBusSteps = new();
    private readonly ConfigModel config = ConfigAccessor.GetApplicationConfiguration();

    public async Task InitializeAsync()
    {
        await serviceBusSteps.ClearTopicAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }


// TODO: if the step ConfirmEventIsPresentInPendingQueue should be from cosmosdbsteps or servicebussteps
    [Theory]
    [InlineData("id", "operationCode", "correlationId", "entityId", "eTag")]
    public void Confirm_event_is_published_when_document_is_added_to_cosmos(
            string id, 
            string operationCode, 
            string correlationId, 
            string entityId, 
            string eTag)
    {
        this.Given(step => serviceBusSteps.SubscribeToTheServiceBusTopic())
            .And(step => cosmosDbSteps.GivenCosmosDbDocumentIsCreated())
            .When(step => cosmosDbSteps.WhenItemIsAddedToCosmosDb(id, operationCode, correlationId, entityId, eTag))
            .And(step => serviceBusSteps.AddAValidEventToServiceBus())
            .Then(step => serviceBusSteps.ConfirmEventIsPresentInPendingQueue())
            .And(step => serviceBusSteps.ConfirmEventIsProcessedByFunctionApp())
            .And(step => serviceBusSteps.ConfirmEventIsNotPresentInDeadLetter())
            .BDDfy();
    }

    public void Confirm_event_is_published_when_document_is_updated_to_cosmos(
        string id, 
        string operationCode, 
        string correlationId, 
        string entityId, 
        string eTag)
    {
        this.Given(step => serviceBusSteps.SubscribeToTheServiceBusTopic())
            .And(step => cosmosDbSteps.GivenCosmosDbDocumentIsCreated())
            .When(step => cosmosDbSteps.WhenItemIsUpdatedInCosmosDb(id, operationCode, correlationId, entityId, eTag))
            .And(step => serviceBusSteps.AddAValidEventToServiceBus())
            .Then(step => serviceBusSteps.ConfirmEventIsPresentInPendingQueue())
            .Then(step => serviceBusSteps.ConfirmEventIsProcessedByFunctionApp())
            .And(step => serviceBusSteps.ConfirmEventIsNotPresentInDeadLetter())
            .BDDfy();
    }
//TODO: check how to complete the test below, since all the canges in DB go to DeadLetter
public void Confirm_invalid_event_is_not_received_by_function_app(string invalidProperty)
    {
        this.Given(s => serviceBusSteps.SubscribeToTheServiceBusTopic())
            .When(s => serviceBusSteps.AddAInValidEventToServiceBus(invalidProperty))
            .Then(s => serviceBusSteps.ConfirmEventIsPresentInDeadLetter())
            .BDDfy();
    }

    
}
