using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Stories;

//Define the story/feature being tested
[Story(
    AsA = "restaurant administrator",
    IWant = "to be able to create menus with categories and items",
    SoThat = "customers know what we have on offer")]
public class CreateItemTests : IClassFixture<AuthFixture>
{
    private readonly ItemSteps itemSteps;
    private readonly AuthFixture fixture;


    public CreateItemTests(AuthFixture fixture)
    {
        //Get instances of the fixture and steps required for the test
        this.fixture = fixture;
        itemSteps = new ItemSteps();
    }
        
    //Add all tests that make up the story to this class.
    [Fact]
    public void Create_item_for_menu()
    {
        this.Given(step => fixture.GivenAUser())
            .And(step => itemSteps.GivenIHaveSpecfiedAFullItem())
            .When(step => itemSteps.WhenICreateTheItemForAnExistingMenuAndCategory())
            .Then(step => itemSteps.ThenTheItemHasBeenCreated())
            //.Then(step => itemSteps.ThenSomeActionIsMade())
            //This step is to verify the outcome of the event in the Subcriber. (e.g. a field is updated in DB, blob is created and so on).
            .BDDfy();
    }
}