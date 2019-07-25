using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models;
using xxAMIDOxx.xxSTACKSxx.Consumer.PactTests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Consumer.PactTests
{
    public class MenuApiPactTests : IClassFixture<MenuApiMock>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public MenuApiPactTests(MenuApiMock fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task Given_Valid_Menu_Request_MenuContent_Should_Be_Returned()
        {
            var menuId = Guid.Parse("9dbffe96-daa5-4adc-a888-34e41dc205d4");

            //var categoriesList = new List<Category>
            //{
            //    new Category {
            //        Id =   Guid.Parse("13d763ba-d784-431a-aad1-f25e2974e728"),
            //        Name = "",
            //        Description = "",
            //        Items = new List<MenuItem>()
            //    },
            //    new Category {
            //        Id =   Guid.Parse("13d763ba-d784-431a-aad1-f25e2974e728"),
            //        Name = "",
            //        Description = "",
            //        Items = new List<MenuItem>()
            //    }
            //};

            _mockProviderService
                .Given("Existing menu")
                .UponReceiving("A GET request to receive a menu by Id")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/v1/menu/{menuId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new Menu
                    {
                        Id = menuId,
                        Name = "menu tuga",
                        Description = "pastel de nata",
                        Enabled = true,
                        Categories = null
                    }
                });

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_mockProviderServiceBaseUri}/v1/menu/{menuId}");
            var json = await response.Content.ReadAsStringAsync();
            var menuResult = JsonConvert.DeserializeObject<Menu>(json);

            menuResult.Id.ShouldBe(menuId);

            _mockProviderService.VerifyInteractions();
            _mockProviderService.ClearInteractions();
        }
    }
}
