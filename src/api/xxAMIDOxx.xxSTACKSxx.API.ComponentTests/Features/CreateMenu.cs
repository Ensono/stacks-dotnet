using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests
{
    [Trait("TestType", "ComponentTests")]
    public class CreateMenu
    {
        /* SCENARIO: Create a menu

            | AsRole     | Validation   | Outcome              |
            |------------|--------------|----------------------|
            | Admin      | Valid Menu   | 201 Resource Create  |
            | Employee   | Valid Menu   | 403 Forbidden        |
            | Customer   | Valid Menu   | 403 Forbidden        |

        */

        [Fact]
        public void CreateMenuAsAdminGivenAValidMenuWhenSubmittedShouldBeSuccesfulAndReturnAResourceCreatedResponse()
        {
            //var client = new HttpClient();
            //client.AsAdmin();
            //client.PostAsync("/")

            //"As an Admin".x();
            //"Given a valid menu being submitted"
            //"And the menu does not does not exist"
            //"When the menu is submitted";
            //"Then a successful response is returned"
        }
    }
}
