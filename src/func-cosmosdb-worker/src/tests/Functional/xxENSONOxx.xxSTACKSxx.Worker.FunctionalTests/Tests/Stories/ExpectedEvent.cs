using TestStack.BDDfy;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "ECommerce Manager",
    IWant = "to know when a payment has been sent",
    SoThat = "so that goods can be sent")]
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
