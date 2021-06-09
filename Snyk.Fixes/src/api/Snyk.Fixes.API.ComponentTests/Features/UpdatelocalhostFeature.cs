using Xunit;

namespace Snyk.Fixes.API.ComponentTests.Features
{
    [Trait("TestType", "ComponentTests")]
    public class UpdatelocalhostFeature
    {
        /* SCENARIOS: Update a localhost
          
             Examples: 
             -----------------------------------------------------------------------------------
            | AsRole                        | localhost Condition             | Outcome              |
            |-------------------------------|----------------------------|----------------------|
            | Admin, Employee               | Valid localhost                 | 204 No Content       |
            | Admin, Employee               | localhost from other restaurant | 404 Not found        |
            | Admin, Employee               | Invalid localhost               | 400 Bad  Request     | 
            | Customer, UnauthenticatedUser | Valid localhost                 | 403 Forbidden        |

        */

        //TODO: Implement test scenarios
    }
}
