using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator for a restaurant",
    IWant = "To be able to update existing items",
    SoThat = "The menus are always showing our latest offerings"
)]
public class UpdateItemById(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly ItemSteps itemSteps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Update_Existing_Items()
    {
        this.Given(s => fixture.GivenAnAdmin())
            .And(step => itemSteps.GivenIHaveSpecfiedAFullItem())
            .When(step => itemSteps.WhenICreateTheItemForAnExistingMenuAndCategory())
            .Then(step => itemSteps.ThenTheItemHasBeenCreated())
            .When(s => itemSteps.WhenISendAnUpdateItemRequest())
            .Then(s => itemSteps.ThenTheItemIsUpdatedCorrectly())
            .BDDfy();
    }
}
