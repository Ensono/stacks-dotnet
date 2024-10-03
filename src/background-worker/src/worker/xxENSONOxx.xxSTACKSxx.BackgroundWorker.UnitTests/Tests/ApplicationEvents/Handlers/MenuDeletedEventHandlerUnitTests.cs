using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class MenuDeletedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<MenuDeletedEventHandler> logger;
    private readonly MenuDeletedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing MenuDeletedEventHandler...";


    public MenuDeletedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<MenuDeletedEventHandler>();
        systemUnderTest = new MenuDeletedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<MenuDeletedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
