using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator for a restaurant",
        IWant = "To be able to update existing items",
        SoThat = "The menus are always showing our latest offerings"
        )]
    public class UpdateItemById : IClassFixture<AuthFixture>
    {
        private readonly AuthFixture fixture;
        private readonly ItemSteps itemSteps;

        public UpdateItemById(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            itemSteps = new ItemSteps();
        }

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
}
