using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Functional
{
    [Story(AsA = "Administrator of a restaurant",
        IWant = "To be able to delete old menus",
        SoThat = "Customers do not see out of date options")]
    public class DeleteMenuTests : BaseSteps, IClassFixture<MenuFixture>
    {
        private readonly MenuFixture fixture;
        private HttpResponseMessage response;

        public DeleteMenuTests(MenuFixture fixture)
        {
            this.fixture = fixture;
        }

        async Task WhenIDeleteAMenu()
        {
            response = await fixture.service.Delete($"/menu/v1/{existingMenu.id}");
        }

        void ThenTheMenuHasBeenDeleted()
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //ToDo: Assert the DB
        }

        [Fact]
        public void Admins_Can_Delete_Menus()
        {
            this.Given(s => s.GivenIAmAUser())
                .And(s => s.GivenAMenuAlreadyExists())
                .When(s => s.WhenIDeleteAMenu())
                .Then(s => s.ThenTheMenuHasBeenDeleted())
                .BDDfy();
        }
    }
}
