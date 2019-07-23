using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator for a restaurant",
        IWant = "To be able to update existing menus",
        SoThat = "I the menus are always showing our latest offerings"
        )]
    public class UpdateMenuById : IClassFixture<AuthFixture>
    {
        private readonly AuthFixture fixture;
        private readonly MenuSteps steps;

        public UpdateMenuById(AuthFixture fixture)
        {
            this.fixture = fixture;
            steps = new MenuSteps();
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Admins_Can_Update_Existing_Menus()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(s => steps.GivenAMenuAlreadyExists())
                .When(s => steps.WhenISendAnUpdateMenuRequest())
                .Then(s => steps.ThenTheMenuIsUpdatedCorrectly())
                .BDDfy();
        }
    }
}
