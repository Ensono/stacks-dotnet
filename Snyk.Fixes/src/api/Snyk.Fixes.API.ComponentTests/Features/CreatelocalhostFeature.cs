using Xbehave;
using Xunit;
using Snyk.Fixes.API.ComponentTests.Fixtures;
using Snyk.Fixes.CQRS.ApplicationEvents;

namespace Snyk.Fixes.API.ComponentTests.Features
{
    [Trait("TestType", "ComponentTests")]
    public class CreatelocalhostFeature
    {
        /* SCENARIOS: Create a localhost
          
             Examples: 
             -----------------------------------------------------------------
            | AsRole              | localhost Condition     | Outcome              |
            |---------------------|--------------------|----------------------|
            | Admin               | Valid localhost         | 201 Resource Create  |
            | Admin               | Invalid localhost       | 400 Bad  Request     |
            | Employee, Customer,                                             |
            | UnauthenticatedUser | Valid localhost         | 403 Forbidden        |

        */

        [Scenario, CustomAutoData]
        public void CreatelocalhostAsAdminForValidlocalhostShouldSucceed(CreatelocalhostFixture fixture)
        {
            "Given the user is authenticated and has the Admin role".x(() => fixture.GivenTheUserIsAnAuthenticatedAdministrator());
            "And a valid localhost being submitted".x(fixture.GivenAValidlocalhost);
            "And the localhost does not does not exist".x(fixture.GivenAlocalhostDoesNotExist);
            "When the localhost is submitted".x(fixture.WhenThelocalhostCreationIsSubmitted);
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the response code is CREATED".x(fixture.ThenACreatedResponseIsReturned);
            "And the id of the new localhost is returned".x(fixture.ThenTheResourceCreatedResponseIsReturned);
            "And the localhost data is submitted correctly to the database".x(fixture.ThenThelocalhostIsSubmittedToDatabase);
            $"And an event of type {nameof(localhostCreated)} is raised".x(fixture.ThenAlocalhostCreatedEventIsRaised);
        }

        [Scenario, CustomAutoData]
        public void CreatelocalhostAsAdminForInvalidlocalhostShouldFail(CreatelocalhostFixture fixture)
        {
            "Given the user is authenticated and has the Admin role".x(() => fixture.GivenTheUserIsAnAuthenticatedAdministrator());
            "And a valid localhost being submitted".x(fixture.GivenAInvalidlocalhost);
            "And the localhost does not does not exist".x(fixture.GivenAlocalhostDoesNotExist);
            "When the localhost is submitted".x(fixture.WhenThelocalhostCreationIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the localhost is not submitted to the database".x(fixture.ThenThelocalhostIsNotSubmittedToDatabase);
            $"And an event of type {nameof(localhostCreated)} should not be raised".x(fixture.ThenAlocalhostCreatedEventIsNotRaised);
        }

        [Scenario(Skip = "Only works when Auth is implemented")]
        [CustomInlineAutoData("Employee")]
        [CustomInlineAutoData("Customer")]
        [CustomInlineAutoData("UnauthenticatedUser")]
        public void CreatelocalhostAsNonAdminForValidlocalhostShouldFail(string role, CreatelocalhostFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And a valid localhost being submitted".x(fixture.GivenAValidlocalhost);
            "And the localhost does not does not exist".x(fixture.GivenAlocalhostDoesNotExist);
            "When the localhost is submitted".x(fixture.WhenThelocalhostCreationIsSubmitted);
            "Then a Forbidden response is returned".x(fixture.ThenAForbiddenResponseIsReturned);
            "And the localhost is not submitted to the database".x(fixture.ThenThelocalhostIsNotSubmittedToDatabase);
            $"And an event of type {nameof(localhostCreated)} should not be raised".x(fixture.ThenAlocalhostCreatedEventIsNotRaised);
        }
    }
}
