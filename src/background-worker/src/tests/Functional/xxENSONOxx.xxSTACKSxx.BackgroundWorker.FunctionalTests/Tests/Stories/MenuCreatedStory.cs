using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Tests.Stories;

[Story(
    AsA = "Kitchen stock manager",
    IWant = "to know when a new menu is created",
    SoThat = "I can place an order for new ingredients")]
public class MenuCreatedStory : IAsyncLifetime
{
    private readonly MenuCreatedSteps _menuCreatedSteps = new();

    public async Task InitializeAsync()
    {
        await _menuCreatedSteps.ClearTopicAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Confirm_valid_new_menu_event_is_processed_by_worker_service()
    {
        this.Given(s => _menuCreatedSteps.CheckThatTopicExists(), "Given an existing Azure Service Bus Topic")
             .When(s => _menuCreatedSteps.AddAValidEventToServiceBus(), "When a valid 'Menu Created' event is sent to the Service Bus")
             //.Then(s => _menuCreatedSteps.ConfirmEventHasBeenProcessedByWorker(), "The event is processed by the Worker")
             // .And(s => _menuCreatedSteps.ConfirmEventIsNotPresentInDeadLetter(), "The event is not sent to dead-letter.")
            .BDDfy();
    }

    [Fact]
    public void Confirm_invalid_new_menu_event_is_not_processed_by_worker_service()
    {
        this.Given(s => _menuCreatedSteps.CheckThatTopicExists(), "Given an existing Azure Service Bus Topic")
             .When(s => _menuCreatedSteps.AddAInvalidEventToServiceBus(), "When an invalid 'Menu Created' event is sent to the Service Bus")
             //.Then(s => _menuCreatedSteps.ConfirmEventIsPresentInDeadLetter(), "The event is sent to dead-letter.")
            .BDDfy();
    }
}
