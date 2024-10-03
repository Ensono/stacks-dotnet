using xxENSONOxx.xxSTACKSxx.Application.CQRS.Events;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Handlers;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Doubles;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests.Tests.Handlers;

[Trait("TestType", "UnitTests")]
public sealed class CategoryUpdatedEventHandlerUnitTests
{
    private readonly IFixture autoFixture;
    private readonly MockLogger<CategoryUpdatedEventHandler> logger;
    private readonly CategoryUpdatedEventHandler systemUnderTest;
    private const string BeginningExecutionLogMessage = "Executing CategoryUpdatedEventHandler...";


    public CategoryUpdatedEventHandlerUnitTests()
    {
        autoFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        logger = new MockLogger<CategoryUpdatedEventHandler>();
        systemUnderTest = new CategoryUpdatedEventHandler(logger);
    }


    [Fact]
    public void HandleAsync_ShouldLogThatItIsBeginningExecution_WhenCalledWithValidEvent()
    {
        // Arrange
        var applicationEvent = autoFixture.Create<CategoryUpdatedEvent>();

        // Act
        systemUnderTest.HandleAsync(applicationEvent);

        // Assert
        logger.LogMessages.Should().Contain(BeginningExecutionLogMessage);
    }
}
