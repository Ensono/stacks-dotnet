using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator of a restaurant",
        IWant = "To be able to delete old menus",
        SoThat = "Customers do not see out of date options")]
    public class DeleteMenuTests : IClassFixture<AuthFixture>
    {
        private readonly MenuSteps steps;
        private readonly AuthFixture fixture;

        public DeleteMenuTests(AuthFixture fixture)
        {
            this.fixture = fixture;
            steps = new MenuSteps();
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Admins_Can_Delete_Menus()
        {
            this.Given(step => fixture.GivenAUser())
                .And(step => steps.GivenAMenuAlreadyExists())
                .When(step => steps.WhenIDeleteAMenu())
                .Then(step => steps.ThenTheMenuHasBeenDeleted())
                .BDDfy();
        }
    }
}
