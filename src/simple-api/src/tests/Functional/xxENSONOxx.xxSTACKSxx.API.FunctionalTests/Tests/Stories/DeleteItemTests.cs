using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator of a restaurant",
    IWant = "To be able to delete old items",
    SoThat = "Customers do not see out of date options")]
public class DeleteItemTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly ItemSteps itemSteps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Delete_Items()
    {
        this.Given(step => fixture.GivenAUser())
            .And(step => itemSteps.GivenIHaveSpecfiedAFullItem())
            .When(step => itemSteps.WhenICreateTheItemForAnExistingMenuAndCategory())
            .Then(step => itemSteps.ThenTheItemHasBeenCreated())
            .When(step => itemSteps.WhenIDeleteTheItem())
            .Then(step => itemSteps.ThenTheItemHasBeenDeleted())
            .BDDfy();
    }
}
