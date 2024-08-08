using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class UpdateMenuFeature
{
    /* SCENARIOS: Update a menu

         Examples:
         -----------------------------------------------------------------------------------
        | AsRole                        | Menu Condition             | Outcome              |
        |-------------------------------|----------------------------|----------------------|
        | Admin, Employee               | Valid Menu                 | 204 No Content       |
        | Admin, Employee               | Menu from other restaurant | 404 Not found        |
        | Admin, Employee               | Invalid Menu               | 400 Bad  Request     |
        | Customer, UnauthenticatedUser | Valid Menu                 | 403 Forbidden        |

    */

    //TODO: Implement test scenarios
}
