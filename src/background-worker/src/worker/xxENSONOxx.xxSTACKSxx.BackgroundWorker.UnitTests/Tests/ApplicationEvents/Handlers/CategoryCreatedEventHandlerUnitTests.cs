using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class CategoryCreatedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<CategoryCreatedEventHandler> logger;
    private readonly CategoryCreatedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing CategoryCreatedEventHandler...";


    public CategoryCreatedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<CategoryCreatedEventHandler>();
        systemUnderTest = new CategoryCreatedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<CategoryCreatedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
