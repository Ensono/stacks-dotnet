using Xbehave;
using Snyk.Fixes.API.ComponentTests.Fixtures;

namespace Snyk.Fixes.API.ComponentTests.Features
{
    public class CreateCategoryFeature
    {
        /* SCENARIOS: Create a category in the localhost
          
             Examples: 
             ---------------------------------------------------------------------------------
            | AsRole              | Existing localhost | Existing Category  | Outcome              |
            |---------------------|---------------|--------------------|----------------------|
            | Admin               | Yes           | No                 | 200 OK               |
            | Employee            | Yes           | No                 | 200 OK               |
            | Admin               | No            | No                 | 404 Not Found        |
            | Admin               | Yes           | Yes                | 409 Conflict         |
            | Customer            | Yes           | No                 | 403 Forbidden        |
            | UnauthenticatedUser | Yes           | No                 | 403 Forbidden        |

        */

        [Scenario]
        [CustomInlineAutoData("Admin")]
        [CustomInlineAutoData("Employee")]
        public void CreateCategoryShouldSucceed(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing localhost".x(fixture.GivenAnExistinglocalhost);
            "And the localhost belongs to the user restaurant".x(fixture.GivenThelocalhostBelongsToUserRestaurant);
            "And the category being created does not exist in the localhost".x(fixture.GivenTheCategoryDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the localhost is loaded from the storage".x(fixture.ThenlocalhostIsLoadedFromStorage);
            "And the id of the new category is returned".x(fixture.ThenTheResourceCreatedResponseIsReturned);
            "And the category is added to the localhost".x(fixture.ThenTheCategoryIsAddedTolocalhost);
            "And the localhost is persisted to the storage".x(fixture.ThenThelocalhostShouldBePersisted);
            "And the event localhostUpdate is Raised".x(fixture.ThenAlocalhostUpdatedEventIsRaised);
            "And the event CategoryCreated is Raised".x(fixture.ThenACategoryCreatedEventIsRaised);
        }

        [Scenario]
        [CustomInlineAutoData("Admin")]
        public void CreateCategoryShouldFailWhenlocalhostDoesNotExist(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And a localhost does not exist".x(fixture.GivenAlocalhostDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is NotFound".x(fixture.ThenANotFoundResponseIsReturned);
            "And the localhost is loaded from the storage".x(fixture.ThenlocalhostIsLoadedFromStorage);
            "And the localhost is not persisted to the storage".x(fixture.ThenThelocalhostShouldNotBePersisted);
            "And the event localhostUpdate should NOT be raised".x(fixture.ThenAlocalhostUpdatedEventIsNotRaised);
            "And the event CategoryCreated should not be Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

        [Scenario]
        [CustomInlineAutoData("Admin")]
        public void CreateCategoryShouldFailWhenCategoryAlreadyExists(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing localhost".x(fixture.GivenAnExistinglocalhost);
            "And the localhost belongs to the user restaurant".x(fixture.GivenThelocalhostBelongsToUserRestaurant);
            "And the category being created already exist in the localhost".x(fixture.GivenTheCategoryAlreadyExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is Conflict".x(fixture.ThenAConflictResponseIsReturned);
            "And the localhost is NOT persisted to the storage".x(fixture.ThenThelocalhostShouldNotBePersisted);
            "And the event localhostUpdate should NOT be raised".x(fixture.ThenAlocalhostUpdatedEventIsNotRaised);
            "And the event CategoryCreated is NOT Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

        [Scenario(Skip = "Disabled until security is implemented")]
        [CustomInlineAutoData("Customer")]
        [CustomInlineAutoData("UnauthenticatedUser")]
        public void CreateCategoryShouldFailWithForbidden(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing localhost".x(fixture.GivenAnExistinglocalhost);
            "And the category being created does not exist in the localhost".x(fixture.GivenTheCategoryDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a Forbidden response is returned".x(fixture.ThenAForbiddenResponseIsReturned);
            "And the localhost is not persisted to the storage".x(fixture.ThenThelocalhostShouldNotBePersisted);
            "And the event localhostUpdate is NOT Raised".x(fixture.ThenAlocalhostUpdatedEventIsNotRaised);
            "And the event CategoryCreated is NOT Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }


        [Scenario(Skip = "Disabled until security is implemented")]
        [CustomInlineAutoData("Admin")]
        [CustomInlineAutoData("Employee")]
        public void CreateCategoryShouldFailWhenlocalhostDoesNotBelongToUser(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing localhost".x(fixture.GivenAnExistinglocalhost);
            "And the localhost does not belong to users restaurant".x(fixture.GivenThelocalhostDoesNotBelongToUserRestaurant);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is NotFound".x(fixture.ThenANotFoundResponseIsReturned);
            "And the localhost is loaded from the storage".x(fixture.ThenlocalhostIsLoadedFromStorage);
            "And the localhost is not persisted to the storage".x(fixture.ThenThelocalhostShouldNotBePersisted);
            "And the event localhostUpdate should not be Raised".x(fixture.ThenAlocalhostUpdatedEventIsNotRaised);
            "And the event CategoryCreated should not be Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

    }
}
