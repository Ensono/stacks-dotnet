using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
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

        void GivenAMenuAlreadyExists()
        {
            var builder = new MenuBuilder();
            menu = builder
                .SetDefaultValues("Yumido Test Menu")
                .Build();

            //ToDo: Inject menu into Database  
        }

        async Task WhenIGetAMenu()
        {
            response = await fixture.service.Get($"v1/menu/{menu.id}");
        }

        async Task ThenICanViewTheMenu()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK, $"Get menu request was not successful. Actual response: {response.StatusCode}");

            //ToDo: Uncomment when we have DB
            //var responseMenu = JsonConvert.DeserializeObject<Menu>(await response.Content.ReadAsStringAsync());

            //Assert.Equal(menu.id, responseMenu.id);
            //Assert.Equal(menu.name, responseMenu.name);
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
