using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]
    public class CreateMenuStory : BaseStory, IClassFixture<MenuFixture>
    {
        private Menu menu;
        private HttpResponseMessage response;
        private readonly MenuFixture fixture;

        public CreateMenuStory(MenuFixture fixture)
        {
            this.fixture = fixture;
            Debug.WriteLine("CreateMenu Constructor");
        }

        void GivenIHaveSpecfiedAFullMenu()
        {
            var builder = new MenuBuilder();

            menu = builder.CreateTestMenu("Yumido Test Menu")
                .Build();
        }

        void GivenIHaveADraftMenu()
        {
            var builder = new MenuBuilder();

            menu = builder.CreateTestMenu("Yumido Test Menu")
                .SetEnabled(false)
                .Build();
        }


        async Task WhenICreateTheMenu()
        {
            //Todo: Add authentication to requests (bearer xyz)
            var menuAsJson = JsonConvert.SerializeObject(menu);

            response = await fixture.service.PostJson("v1/menu/", menuAsJson);
        }

        void ThenTheMenuHasBeenSuccessfullyPublished()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
            //Check the menu is in the DB
        }

        [Fact]
        public void Admins_Can_Publish_A_New_Menu_To_Yumido()
        {
            this.Given(s => s.GivenIHaveSpecfiedAFullMenu())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }

        [Fact]
        public void Admins_Can_Create_A_Draft_Menu()
        {
            this.Given(s => s.GivenIHaveADraftMenu())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }
    }
}
