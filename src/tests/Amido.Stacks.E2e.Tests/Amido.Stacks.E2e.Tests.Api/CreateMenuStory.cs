using Amido.Stacks.E2e.Tests.Api.Builders;
using Amido.Stacks.E2e.Tests.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.E2e.Tests.Api
{
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create menus",
        SoThat = "customers know what we have on offer")]
    public class CreateMenuStory
    {
        private CrudService service;
        private Menu menu;
        private HttpResponseMessage response;

        void GivenIHaveSpecfiedAMenuWithNoCategories()
        {
            var builder = new MenuBuilder();

            menu = builder.CreateTestMenu("Yumido Test Menu")
                .WithNoCategories()
                .Build();
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
            var menuAsJson = JsonConvert.SerializeObject(menu);

            service = new CrudService("https://virtserver.swaggerhub.com");
            response = await service.PostJson("/guibirow/Yumido/v1/menu/", menuAsJson);
        }

        void ThenTheMenuHasBeenSuccessfullyPublished()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public void Admin_Can_Create_Menu_With_No_Categories()
        {
            this.Given(s => s.GivenIHaveSpecfiedAMenuWithNoCategories())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }

        [Fact]
        public void Admin_Can_Create_A_Full_Menu()
        {
            this.Given(s => s.GivenIHaveADraftMenu())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }

        [Fact]
        public void Admin_Can_Create_A_Draft_Menu()
        {
            this.Given(s => s.GivenIHaveSpecfiedAFullMenu())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }
    }
}
