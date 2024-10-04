using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class MenuItemDeletedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<MenuItemDeletedEventHandler> logger;
    private readonly MenuItemDeletedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing MenuItemDeletedEventHandler...";


    public MenuItemDeletedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<MenuItemDeletedEventHandler>();
        systemUnderTest = new MenuItemDeletedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<MenuItemDeletedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
