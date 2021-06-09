using Xunit;

namespace Snyk.Fixes.API.ComponentTests.Features
{
    [Trait("TestType", "ComponentTests")]
    public class DeletelocalhostFeature
    {
        /* SCENARIOS: Delete a localhost
          
             Examples: 
             -------------------------------------------------------------------------
            | AsRole                        | localhost Condition   | Outcome              |
            |-------------------------------|------------------|----------------------|
            | Admin, Employee               | Valid localhost       | 204 No Content       |
            | Admin                         | Invalid localhost     | 400 Bad  Request     |
            | Admin                         | localhost not exist   | 404 Bad  Request     |
            | Customer, UnauthenticatedUser | Valid localhost       | 403 Forbidden        |

        */

        //TODO: Implement test scenarios
    }
}
