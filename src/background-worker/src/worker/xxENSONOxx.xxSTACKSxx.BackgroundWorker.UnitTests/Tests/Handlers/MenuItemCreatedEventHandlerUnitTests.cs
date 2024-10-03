using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class MenuItemCreatedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<MenuItemCreatedEventHandler> logger;
    private readonly MenuItemCreatedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing MenuItemCreatedEventHandler...";


    public MenuItemCreatedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<MenuItemCreatedEventHandler>();
        systemUnderTest = new MenuItemCreatedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<MenuItemCreatedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
