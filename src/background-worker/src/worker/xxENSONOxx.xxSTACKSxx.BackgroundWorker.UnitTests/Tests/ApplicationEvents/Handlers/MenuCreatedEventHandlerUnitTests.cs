using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

//[Trait("TestType", "UnitTests")]
//public sealed class MenuCreatedEventHandlerUnitTests
//{
//    private readonly IFixture autoFixture;
//    private readonly MockLogger<MenuCreatedEventHandler> logger;
//    private readonly MenuCreatedEventHandler systemUnderTest;
//    private const string BeginningExecutionLogMessage = "Executing MenuCreatedEventHandler...";


//    public MenuCreatedEventHandlerUnitTests()
//    {
//        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
//        logger = new MockLogger<MenuCreatedEventHandler>();
//        systemUnderTest = new MenuCreatedEventHandler(logger);
//    }


//    [Fact]
//    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
//    {
//        // Arrange
//        var applicationEvent = autoFixture.Create<MenuCreatedEvent>();

//        // Act
//        systemUnderTest.HandleAsync(applicationEvent);

//        // Assert
//        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
//    }
//}
