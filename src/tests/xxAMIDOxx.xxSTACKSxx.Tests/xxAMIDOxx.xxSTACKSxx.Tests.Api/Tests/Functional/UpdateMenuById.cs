using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    [Story(AsA = "Administrator for a restaurant",
        IWant = "To be able to update existing menus",
        SoThat = "I the menus are always showing our latest offerings"
        )]
    public class UpdateMenuById : BaseSteps, IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;
        private HttpResponseMessage response;

        public UpdateMenuById(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        async Task WhenISendAnUpdateMenuRequest()
        {
            var requestObject = new MenuRequestBuilder()
                .WithName("Updated Menu Name")
                .WithDescription("Updated Description")
                .SetEnabled(true)
                .Build();

            var jsonRequest = JsonConvert.SerializeObject(requestObject);

            response = await fixture.service.PutJson($"v1/menu/{existingMenu.id}", jsonRequest);
        }

        void ThenTheMenuIsUpdatedCorrectly()
        {
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            //ToDo: Check menu is in Database with updated values
        }

        [Fact]
        public void Admins_Can_Update_Existing_Menus()
        {
            this.Given(s => s.GivenIAmAnAdmin())
                .And(s => s.GivenAMenuAlreadyExists())
                .When(s => s.WhenISendAnUpdateMenuRequest())
                .Then(s => s.ThenTheMenuIsUpdatedCorrectly())
                .BDDfy();
        }
    }
}
