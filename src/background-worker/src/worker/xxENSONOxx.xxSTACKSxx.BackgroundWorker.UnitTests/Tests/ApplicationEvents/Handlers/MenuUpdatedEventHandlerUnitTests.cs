using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class MenuUpdatedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<MenuUpdatedEventHandler> logger;
    private readonly MenuUpdatedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing MenuUpdatedEventHandler...";


    public MenuUpdatedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<MenuUpdatedEventHandler>();
        systemUnderTest = new MenuUpdatedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<MenuUpdatedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
