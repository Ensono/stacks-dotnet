using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator for a restaurant",
    IWant = "To be able to update existing menus",
    SoThat = "The menus are always showing our latest offerings"
)]
public class UpdateMenuById(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly MenuSteps steps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Update_Existing_Menus()
    {
        this.Given(s => fixture.GivenAnAdmin())
            .And(s => steps.GivenAMenuAlreadyExists())
            .When(s => steps.WhenISendAnUpdateMenuRequest())
            .Then(s => steps.ThenTheMenuIsUpdatedCorrectly())
            //Then(step => categorySteps.ThenSomeActionIsMade())
            //This step is to verify the outcome of the event in the Subcriber. (e.g. a field is updated in DB, blob is created and so on).
            .BDDfy();
    }
}
