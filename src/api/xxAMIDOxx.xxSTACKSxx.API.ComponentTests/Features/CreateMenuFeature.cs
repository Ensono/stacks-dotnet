using System;
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
        /* SCENARIOs: Create a menu

            | AsRole              | Validation   | Outcome              |
            |---------------------|--------------|----------------------|
            | Admin               | Valid Menu   | 201 Resource Create  |
            | Admin               | Invalid Menu | 400 Bad  Request     |
            | Employee            | Valid Menu   | 403 Forbidden        |
            | Customer            | Valid Menu   | 403 Forbidden        |
            | UnauthenticatedUser | Valid Menu   | 403 Forbidden        |

        */

        [Scenario, AutoData]
        public void CreateMenuAsAdminForValidMenuShouldSucceed(CreateMenuFixture fixture)
        {
            "As Admin".x((Action)fixture.AsAdmin);
            "Given a valid menu being submitted".x((Action)fixture.GivenAValidMenu);
            "And the menu does not does not exist".x((Action)fixture.GivenTheMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a successful response is returned".x((Action)fixture.ThenASuccessfulResponseIsReturned);
            "And a check for existing menu is called".x((Action)fixture.ThenGetMenuByIdIsCalled);
            "And the menu data is submitted correctly to the database".x((Action)fixture.ThenTheMenuIsSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreatedEvent).Name} is raised".x((Action)fixture.ThenAMenuCreatedEventIsRaised);
        }

        [Scenario, AutoData]
        public void CreateMenuAsAdminForInvalidMenuShouldFail(CreateMenuFixture fixture)
        {
            "As Admin".x((Action)fixture.AsAdmin);
            "Given a valid menu being submitted".x((Action)fixture.GivenAInvalidMenu);
            "And the menu does not does not exist".x((Action)fixture.GivenTheMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a failure response is returned".x((Action)fixture.ThenAFailureResponseIsReturned);
            "And the menu is not submitted to the database".x((Action)fixture.ThenTheMenuIsNotSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreatedEvent).Name} should not be raised".x((Action)fixture.ThenAMenuCreatedEventIsNotRaised);
        }

        [Scenario(Skip = "Only works when Auth is implemented")]
        [InlineAutoData("Employee")]
        [InlineAutoData("Customer")]
        [InlineAutoData("UnauthenticatedUser")]
        public void CreateMenuAsNonAdminForValidMenuShouldFail(string role, CreateMenuFixture fixture)
        {
            $"As {role}".x(() => fixture.AsRole(role));
            "Given a valid menu being submitted".x((Action)fixture.GivenAValidMenu);
            "And the menu does not does not exist".x((Action)fixture.GivenTheMenuDoesNotExist);
            "When the menu is submitted".x(fixture.WhenTheMenuCreationIsSubmitted);
            "Then a Forbidden response is returned".x((Action)fixture.ThenAForbiddenResponseIsReturned);
            "And the menu is not submitted to the database".x((Action)fixture.ThenTheMenuIsNotSubmittedToDatabase);
            $"And an event of type {typeof(MenuCreatedEvent).Name} should not be raised".x((Action)fixture.ThenAMenuCreatedEventIsNotRaised);
        }
    }
}
