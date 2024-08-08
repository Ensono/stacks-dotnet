using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator for a restaurant",
    IWant = "To be able to update existing menus",
    SoThat = "The menus are always showing our latest offerings"
)]
public class UpdateMenuById : IClassFixture<AuthFixture>
{
    private readonly AuthFixture fixture;
    private readonly MenuSteps steps;

    public UpdateMenuById(AuthFixture fixture)
    {
        //Get instances of the fixture and steps required for the test
        this.fixture = fixture;
        steps = new MenuSteps();
    }

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