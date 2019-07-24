using AutoFixture.Xunit2;
using Xbehave;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Features
{
    [Trait("TestType", "ComponentTests")]
    public class CreateMenuFeature
    {
        /* SCENARIOS: Create a menu
          
             Examples: 
             -----------------------------------------------------------------
            | AsRole              | Menu Condition     | Outcome              |
            |---------------------|--------------------|----------------------|
            | Admin               | Valid Menu         | 201 Resource Create  |
            | Admin               | Invalid Menu       | 400 Bad  Request     |
            | Employee, Customer,                                             |
            | UnauthenticatedUser | Valid Menu         | 403 Forbidden        |

        */

        [Scenario, AutoData]
        public void CreateMenuAsAdminForValidMenuShouldSucceed(CreateMenuFixture fixture)
        {
            "As Admin".x(fixture.AsAdmin);
            "Given a valid menu being submitted".x(fixture.GivenAValidMenu);
            "And the menu does not does not exist".x(fixture.GivenAMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the response code is CREATED".x(fixture.ThenACreatedResponseIsReturned);
            "And the id of the new menu is returned".x(fixture.ThenTheResourceCreatedResponseIsReturned);
            "And the menu data is submitted correctly to the database".x(fixture.ThenTheMenuIsSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreated).Name} is raised".x(fixture.ThenAMenuCreatedEventIsRaised);
        }

        [Scenario, AutoData]
        public void CreateMenuAsAdminForInvalidMenuShouldFail(CreateMenuFixture fixture)
        {
            "As Admin".x(fixture.AsAdmin);
            "Given a valid menu being submitted".x(fixture.GivenAInvalidMenu);
            "And the menu does not does not exist".x(fixture.GivenAMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the menu is not submitted to the database".x(fixture.ThenTheMenuIsNotSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreated).Name} should not be raised".x(fixture.ThenAMenuCreatedEventIsNotRaised);
        }

        [Scenario(Skip = "Only works when Auth is implemented")]
        [InlineAutoData("Employee")]
        [InlineAutoData("Customer")]
        [InlineAutoData("UnauthenticatedUser")]
        public void CreateMenuAsNonAdminForValidMenuShouldFail(string role, CreateMenuFixture fixture)
        {
            $"As {role}".x(() => fixture.AsRole(role));
            "Given a valid menu being submitted".x(fixture.GivenAValidMenu);
            "And the menu does not does not exist".x(fixture.GivenAMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a Forbidden response is returned".x(fixture.ThenAForbiddenResponseIsReturned);
            "And the menu is not submitted to the database".x(fixture.ThenTheMenuIsNotSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreated).Name} should not be raised".x(fixture.ThenAMenuCreatedEventIsNotRaised);
        }
    }
}
