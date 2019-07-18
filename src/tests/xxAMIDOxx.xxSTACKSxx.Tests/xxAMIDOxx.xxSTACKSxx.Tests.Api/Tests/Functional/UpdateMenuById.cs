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
    public class UpdateMenuById : IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;

        public UpdateMenuById(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Admins_Can_Update_Existing_Menus()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(s => fixture.GivenAMenuAlreadyExists())
                .When(s => fixture.WhenISendAnUpdateMenuRequest())
                .Then(s => fixture.ThenTheMenuIsUpdatedCorrectly())
                .BDDfy();
        }
    }
}
