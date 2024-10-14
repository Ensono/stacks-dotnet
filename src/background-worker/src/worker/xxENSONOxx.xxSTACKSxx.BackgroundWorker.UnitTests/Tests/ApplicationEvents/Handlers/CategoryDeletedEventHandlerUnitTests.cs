using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.ApplicationEvents.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.ApplicationEvents.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class CategoryDeletedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<CategoryDeletedEventHandler> logger;
    private readonly CategoryDeletedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing CategoryDeletedEventHandler...";


    public CategoryDeletedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<CategoryDeletedEventHandler>();
        systemUnderTest = new CategoryDeletedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<CategoryDeletedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
