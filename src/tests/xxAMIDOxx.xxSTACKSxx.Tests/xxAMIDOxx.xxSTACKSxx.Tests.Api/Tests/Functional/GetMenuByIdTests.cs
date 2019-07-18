using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(
        AsA = "user of the Yumido website",
        IWant = "to be able to view specific menus",
        SoThat = "I can choose what to eat")]
    public class GetMenuByIdTests : IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;

        public GetMenuByIdTests(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Users_Can_View_Existing_Menus()
        {
            this.Given(s => fixture.GivenAUser())
                .And(s => fixture.GivenAMenuAlreadyExists())
                .When(s => fixture.WhenIGetAMenu())
                .Then(s => fixture.ThenICanViewTheMenu())
                .BDDfy();
        }

        [Fact]
        public void Admins_Can_View_Existing_Menus()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(s => fixture.GivenAMenuAlreadyExists())
                .When(s => fixture.WhenIGetAMenu())
                .Then(s => fixture.ThenICanViewTheMenu())
                .BDDfy();
        }
    }
}
