using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Features
{
    [Trait("TestType", "ComponentTests")]
    public class DeleteMenuFeature
    {
        /* SCENARIOS: Delete a menu
          
             Examples: 
             -------------------------------------------------------------------------
            | AsRole                        | Menu Condition   | Outcome              |
            |-------------------------------|------------------|----------------------|
            | Admin, Employee               | Valid Menu       | 204 No Content       |
            | Admin                         | Invalid Menu     | 400 Bad  Request     |
            | Admin                         | Menu not exist   | 404 Bad  Request     |
            | Customer, UnauthenticatedUser | Valid Menu       | 403 Forbidden        |

        */

        //TODO: Implement test scenarios
    }
}
