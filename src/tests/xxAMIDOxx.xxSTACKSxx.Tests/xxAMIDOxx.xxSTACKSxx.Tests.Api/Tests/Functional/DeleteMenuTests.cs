using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator of a restaurant",
        IWant = "To be able to delete old menus",
        SoThat = "Customers do not see out of date options")]
    public class DeleteMenuTests : IClassFixture<MenuFixture>
    {
        private readonly MenuFixture menuFixture;

        public DeleteMenuTests(MenuFixture menuFixture)
        {
            this.menuFixture = menuFixture;
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Admins_Can_Delete_Menus()
        {
            this.Given(s => menuFixture.GivenAUser())
                .And(s => menuFixture.GivenAMenuAlreadyExists())
                .When(s => menuFixture.WhenIDeleteAMenu())
                .Then(s => menuFixture.ThenTheMenuHasBeenDeleted())
                .BDDfy();
        }
    }
}
