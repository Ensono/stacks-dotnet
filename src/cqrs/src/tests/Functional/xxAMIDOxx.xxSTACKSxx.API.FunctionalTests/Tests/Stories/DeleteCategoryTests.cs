using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator of a restaurant",
    IWant = "To be able to delete old categories",
    SoThat = "Customers do not see out of date options")]
public class DeleteCategoryTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly CategorySteps categorySteps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Delete_Categories()
    {
        this.Given(step => fixture.GivenAUser())
            .And(step => categorySteps.GivenIHaveSpecfiedAFullCategory())
            .When(step => categorySteps.WhenICreateTheCategoryForAnExistingMenu())
            .Then(step => categorySteps.ThenTheCategoryHasBeenCreated())
            .When(step => categorySteps.WhenIDeleteTheCategory())
            .Then(step => categorySteps.ThenTheCategoryHasBeenDeleted())
            //Then(step => steps.ThenSomeActionIsMade())
            //This step is to verify the outcome of the event in the Subcriber. (e.g. a field is updated in DB, blob is created and so on).
            .BDDfy();
    }
}
