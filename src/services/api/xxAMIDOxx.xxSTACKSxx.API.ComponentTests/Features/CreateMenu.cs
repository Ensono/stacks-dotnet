using System;
using System.Net.Http;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests
{
    public class CreateMenu
    {
        /* SCENARIO: Create a menu

            | Role       | Validation   | Outcome              |
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
        }
    }
}
