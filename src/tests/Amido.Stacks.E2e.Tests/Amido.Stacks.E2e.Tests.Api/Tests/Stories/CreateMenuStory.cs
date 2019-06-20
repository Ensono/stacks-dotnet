using Amido.Stacks.E2e.Tests.Api.Builders;
using Amido.Stacks.E2e.Tests.Api.Models;
using Amido.Stacks.E2e.Tests.Api.Tests.Fixtures;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.E2e.Tests.Api.Tests.Stories
{
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]
    public class CreateMenuStory : IClassFixture<MenuFixture>
    {
        private CrudService service;
        private Menu menu;
        private HttpResponseMessage response;
        private readonly MenuFixture fixture;

        public CreateMenuStory(MenuFixture fixture)
        {
            this.fixture = fixture;
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

            service = new CrudService(fixture.configuration.BaseUrl);
            response = await service.PostJson("v1/menu/", menuAsJson);
        }

        void ThenTheMenuHasBeenSuccessfullyPublished()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        void AndTheMenuIsAvailableToUsers()
        {
            //ToDo: Call GET api as a user
        }

        [Fact]
        public void Admins_Can_Publish_A_New_Menu_To_Yumido()
        {
            this.Given(s => s.GivenIHaveSpecfiedAFullMenu())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .And(s => s.AndTheMenuIsAvailableToUsers())
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
