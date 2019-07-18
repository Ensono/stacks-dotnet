using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    //Define the story/feature being tested
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]
    public class CreateMenuTests : IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;

        public CreateMenuTests(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        //Add all tests that make up the story to this class.
        //Steps should be taken from the fixture
        [Fact]
        public void Create_a_menu()
        {
            this.Given(step => fixture.GivenIHaveSpecfiedAFullMenu())
                .When(step => fixture.WhenICreateTheMenu())
                .Then(step => fixture.ThenTheMenuHasBeenCreated())
                .BDDfy();
        }
    }
}
