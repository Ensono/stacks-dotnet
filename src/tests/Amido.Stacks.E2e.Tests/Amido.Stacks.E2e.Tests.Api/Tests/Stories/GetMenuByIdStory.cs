using Amido.Stacks.E2e.Tests.Api.Tests.Fixtures;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.E2e.Tests.Api.Tests.Stories
{
    [Story(
        AsA = "user of the Yumido website",
        IWant = "to be able to view specific menus",
        SoThat = "I can choose what to eat")]
    public class GetMenuByIdStory : IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;

        public GetMenuByIdStory(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        void GivenIAmAUser()
        {

        }

        void AndGivenAMenuAlreadyExists()
        {

        }

        void WhenIGetAMenu()
        {

        }

        void ThenICanViewTheMenu()
        {

        }

        [Fact]
        public void Users_Can_View_Existing_Menus()
        {
        }

        [Fact]
        public void Admins_Can_View_Existing_Menus()
        {

        }
    }
}
