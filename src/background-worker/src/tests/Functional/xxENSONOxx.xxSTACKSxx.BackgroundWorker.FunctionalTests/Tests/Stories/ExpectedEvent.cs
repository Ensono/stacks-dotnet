using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "Menu Manager",
    IWant = "to know when a new menu is created",
    SoThat = "so that I can place an order for ingredients.")]
public class SampleStory : IAsyncLifetime
{
    private readonly ServiceBusSteps _serviceBusSteps = new();

    public async Task InitializeAsync()
    {
        await _serviceBusSteps.ClearTopicAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }


    [Fact]
    public void Confirm_valid_event_is_processed_by_worker_service()
    {
        this.Given(s => _serviceBusSteps.CheckThatTopicExists())
             .When(s => _serviceBusSteps.AddAValidEventToServiceBus())
             .Then(s => _serviceBusSteps.ConfirmEventHasBeenProcessedByWorker())
              .And(s => _serviceBusSteps.ConfirmEventIsNotPresentInDeadLetter())
            .BDDfy();

    }


    [Fact]
    public void Confirm_invalid_event_is_not_processed_by_worker_service()
    {
        this.Given(s => _serviceBusSteps.CheckThatTopicExists())
             .When(s => _serviceBusSteps.AddAInValidEventToServiceBus())
             .Then(s => _serviceBusSteps.ConfirmPaymentIsPresentInDeadLetter())
            .BDDfy();
    }
}
