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

        void GivenIHaveSpecfiedAMenuwithNoCategories()
        {
            var builder = new MenuBuilder();

            menu = builder.WithId(Guid.NewGuid())
                .WithName("TestMenu")
                .SetEnabled(true)
                .WithNoCategories()
                .Build();
        }

        async Task WhenICreateTheMenu()
        {
            var json = JsonConvert.SerializeObject(menu);
            service = new CrudService("https://virtserver.swaggerhub.com");
            response = await service.PostJson("/guibirow/Yumido/v1/menu/", json);
        }

        void ThenTheMenuHasBeenSuccessfullyPublished()
        {
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public void UserCanCreateAMenuWithNoCategories()
        {
            this.Given(s => s.GivenIHaveSpecfiedAMenuwithNoCategories())
                .When(s => s.WhenICreateTheMenu())
                .Then(s => s.ThenTheMenuHasBeenSuccessfullyPublished())
                .BDDfy();
        }
    }
}
