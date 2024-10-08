using TestStack.BDDfy;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "A Stacks User",
    IWant = "to know when a change has been made in Cosmos",
    SoThat = "my downstream functions can react to the change")]
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


// ----- jacks changes start here -----
    [Theory]
    [InlineData("id1", "operationCode", "correlationId", "entityId", "eTag")]
    public void Confirm_event_is_published_when_document_added_to_cosmos(
            string id, 
            string operationCode, 
            string correlationId, 
            string entityId, 
            string eTag)
    {
        this.Given(s => cosmosDbSteps.CreateCosmosDbDocument(id, operationCode, correlationId, entityId, eTag))
            .When(s => cosmosDbSteps.DocumentIsAddedToCosmosDb())
            .Then(s => serviceBusSteps.ConfirmEventIsPresentInPendingQueue()) //If we have an active listener for this event, we will need to check that listener has processed the event instead of checking the pending queue.
            .BDDfy();
    }
// ----- jacks changes end here -----

    [Fact]
    public void Confirm_valid_payment_is_received_by_function_App()
    {
        this.Given(s => serviceBusSteps.SubscribeToTheServiceBusTopic())
            .When(s => serviceBusSteps.AddAValidEventToServiceBus())
            .Then(s => serviceBusSteps.ConfirmEventIsProcessedByFunctionApp())
            .And(s => serviceBusSteps.ConfirmEventIsNotPresentInDeadLetter())
            .BDDfy();
    }

    [Theory]
    [InlineData("transactionType")]
    public void Confirm_invalid_payment_is_not_received_by_function_app(string invalidProperty)
    {
        this.Given(s => serviceBusSteps.SubscribeToTheServiceBusTopic())
            .When(s => serviceBusSteps.AddAInValidEventToServiceBus(invalidProperty))
            .Then(s => serviceBusSteps.ConfirmPaymentEventIsPresentInDeadLetter())
            .BDDfy();
    }
}
