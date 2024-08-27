using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator of a restaurant",
    IWant = "To be able to delete old menus",
    SoThat = "Customers do not see out of date options")]
public class DeleteMenuTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly MenuSteps steps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Delete_Menus()
    {
        this.Given(step => fixture.GivenAUser())
            .And(step => steps.GivenAMenuAlreadyExists())
            .When(step => steps.WhenIDeleteAMenu())
            .Then(step => steps.ThenTheMenuHasBeenDeleted())
            //Then(step => steps.ThenSomeActionIsMade())
            //This step is to verify the outcome of the event in the Subcriber. (e.g. a field is updated in DB).
            .BDDfy();
    }
}
