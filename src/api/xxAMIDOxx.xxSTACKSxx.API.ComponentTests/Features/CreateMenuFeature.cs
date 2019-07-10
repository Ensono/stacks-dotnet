using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Xbehave;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests
{
    [Trait("TestType", "ComponentTests")]
    public class CreateMenuFeature
    {
        /* SCENARIO: Create a menu

            | AsRole     | Validation   | Outcome              |
            |------------|--------------|----------------------|
            | Admin      | Valid Menu   | 201 Resource Create  |
            | Employee   | Valid Menu   | 403 Forbidden        |
            | Customer   | Valid Menu   | 403 Forbidden        |

        */

        [Scenario, AutoData]
        public void CreateMenuAsAdminGivenAValidMenuWhenSubmittedShouldBeSuccesfulAndReturnAResourceCreatedResponse(CreateMenuFixture fixture)
        {
            //var client = new HttpClient();
            //client.AsAdmin();
            //client.PostAsync("/")

            "As an Admin".x(fixture.AsAdmin);
            "Given a valid menu being submitted".x(() => fixture.GivenAValidMenu());
            "And the menu does not does not exist".x(fixture.GivenTheMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
        }
    }

    public class CreateMenuFixture : ApiFixture
    {
        CreateOrUpdateMenu newMenu;

        public CreateMenuFixture(CreateOrUpdateMenu newMenu)
        {
            this.newMenu = newMenu;
        }

        internal void GivenAValidMenu()
        {
            //Fixture.Create a Mennu
            //TODO: 
        }

        internal void GivenTheMenuDoesNotExist()
        {
            //TODO: Arrange the MOQ dependencies
        }

        internal void ThenASuccessfulResponseIsReturned()
        {
            Assert.True(LastResponse.IsSuccessStatusCode);
        }

        internal async Task WhenTheMenuCreationIsSubmitted()
        {
            //throw new NotImplementedException();
            //TODO: submit request
            await CreateMenu(newMenu);
        }
    }
}
