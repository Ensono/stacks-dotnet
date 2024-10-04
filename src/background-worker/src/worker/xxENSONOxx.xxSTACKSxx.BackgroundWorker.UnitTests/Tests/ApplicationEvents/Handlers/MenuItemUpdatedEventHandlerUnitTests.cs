using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class MenuItemUpdatedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<MenuItemUpdatedEventHandler> logger;
    private readonly MenuItemUpdatedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing MenuItemUpdatedEventHandler...";


    public MenuItemUpdatedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<MenuItemUpdatedEventHandler>();
        systemUnderTest = new MenuItemUpdatedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<MenuItemUpdatedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
