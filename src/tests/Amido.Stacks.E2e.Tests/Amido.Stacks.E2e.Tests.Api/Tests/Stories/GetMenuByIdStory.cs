using Amido.Stacks.E2e.Tests.Api.Builders;
using Amido.Stacks.E2e.Tests.Api.Models;
using Amido.Stacks.E2e.Tests.Api.Tests.Fixtures;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.E2e.Tests.Api.Tests.Stories
{
    [Story(
        AsA = "user of the Yumido website",
        IWant = "to be able to view specific menus",
        SoThat = "I can choose what to eat")]
    public class GetMenuByIdStory : BaseStory, IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;
        private HttpResponseMessage response;
        private Menu menu;

        public GetMenuByIdStory(MenuFixture fixture)
        {
            this.fixture = fixture;
            Debug.WriteLine("GetMenu Constructor");
        }

        async Task GivenAMenuAlreadyExists()
        {
            //ToDo: Inject to Database rather than use Post??
            var menuBuilder = new MenuBuilder();

            menu = menuBuilder
                .CreateTestMenu("Test Menu")
                .Build();

            var response2 = await fixture.service.PostJson("v1/menu", JsonConvert.SerializeObject(menu));
        }

        async Task WhenIGetAMenu()
        {
            response = await fixture.service.Get("v1/menu/" + menu.id);
        }

        void ThenICanViewTheMenu()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            //Assert that the content of the Menu is the same as what was created in Given step
        }

        [Fact]
        public void Users_Can_View_Existing_Menus()
        {
            this.Given(s => s.GivenIAmAUser())
                .And(s => s.GivenAMenuAlreadyExists())
                .When(s => s.WhenIGetAMenu())
                .Then(s => s.ThenICanViewTheMenu())
                .BDDfy();
        }

        [Fact]
        public void Admins_Can_View_Existing_Menus()
        {
            this.Given(s => s.GivenIAmAnAdmin())
                .And(s => s.GivenAMenuAlreadyExists())
                .When(s => s.WhenIGetAMenu())
                .Then(s => s.ThenICanViewTheMenu())
                .BDDfy();
        }
    }
}
