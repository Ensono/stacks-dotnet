using Xbehave;
using xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Features
{
    public class CreateCategoryFeature
    {
        /* SCENARIOS: Create a category in the menu
          
             Examples: 
             ---------------------------------------------------------------------------------
            | AsRole              | Existing Menu | Existing Category  | Outcome              |
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
            "And an existing menu".x(fixture.GivenAnExistingMenu);
            "And the menu belongs to the user restaurant".x(fixture.GivenTheMenuBelongsToUserRestaurant);
            "And the category being created does not exist in the menu".x(fixture.GivenTheCategoryDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the menu is loaded from the storage".x(fixture.ThenMenuIsLoadedFromStorage);
            "And the id of the new category is returned".x(fixture.ThenTheResourceCreatedResponseIsReturned);
            "And the category is added to the menu".x(fixture.ThenTheCategoryIsAddedToMenu);
            "And the menu is persisted to the storage".x(fixture.ThenTheMenuShouldBePersisted);
            "And the event MenuUpdate is Raised".x(fixture.ThenAMenuUpdatedEventIsRaised);
            "And the event CategoryCreated is Raised".x(fixture.ThenACategoryCreatedEventIsRaised);
        }

        [Scenario]
        [CustomInlineAutoData("Admin")]
        public void CreateCategoryShouldFailWhenMenuDoesNotExist(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And a menu does not exist".x(fixture.GivenAMenuDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is NotFound".x(fixture.ThenANotFoundResponseIsReturned);
            "And the menu is loaded from the storage".x(fixture.ThenMenuIsLoadedFromStorage);
            "And the menu is not persisted to the storage".x(fixture.ThenTheMenuShouldNotBePersisted);
            "And the event MenuUpdate should NOT be raised".x(fixture.ThenAMenuUpdatedEventIsNotRaised);
            "And the event CategoryCreated should not be Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

        [Scenario]
        [CustomInlineAutoData("Admin")]
        public void CreateCategoryShouldFailWhenCategoryAlreadyExists(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing menu".x(fixture.GivenAnExistingMenu);
            "And the menu belongs to the user restaurant".x(fixture.GivenTheMenuBelongsToUserRestaurant);
            "And the category being created already exist in the menu".x(fixture.GivenTheCategoryAlreadyExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is Conflict".x(fixture.ThenAConflictResponseIsReturned);
            "And the menu is NOT persisted to the storage".x(fixture.ThenTheMenuShouldNotBePersisted);
            "And the event MenuUpdate should NOT be raised".x(fixture.ThenAMenuUpdatedEventIsNotRaised);
            "And the event CategoryCreated is NOT Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

        [Scenario(Skip = "Disabled until security is implemented")]
        [CustomInlineAutoData("Customer")]
        [CustomInlineAutoData("UnauthenticatedUser")]
        public void CreateCategoryShouldFailWithForbidden(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing menu".x(fixture.GivenAnExistingMenu);
            "And the category being created does not exist in the menu".x(fixture.GivenTheCategoryDoesNotExist);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a Forbidden response is returned".x(fixture.ThenAForbiddenResponseIsReturned);
            "And the menu is not persisted to the storage".x(fixture.ThenTheMenuShouldNotBePersisted);
            "And the event MenuUpdate is NOT Raised".x(fixture.ThenAMenuUpdatedEventIsNotRaised);
            "And the event CategoryCreated is NOT Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }


        [Scenario(Skip = "Disabled until security is implemented")]
        [CustomInlineAutoData("Admin")]
        [CustomInlineAutoData("Employee")]
        public void CreateCategoryShouldFailWhenMenuDoesNotBelongToUser(string role, CreateCategoryFixture fixture)
        {
            $"Given the user is authenticated and has the {role} role".x(() => fixture.GivenTheUserIsAuthenticatedAndHasRole(role));
            "And an existing menu".x(fixture.GivenAnExistingMenu);
            "And the menu does not belong to users restaurant".x(fixture.GivenTheMenuDoesNotBelongToUserRestaurant);
            "When a new category is submitted".x(fixture.WhenTheCategoryIsSubmitted);
            "Then a failure response is returned".x(fixture.ThenAFailureResponseIsReturned);
            "And the response code is NotFound".x(fixture.ThenANotFoundResponseIsReturned);
            "And the menu is loaded from the storage".x(fixture.ThenMenuIsLoadedFromStorage);
            "And the menu is not persisted to the storage".x(fixture.ThenTheMenuShouldNotBePersisted);
            "And the event MenuUpdate should not be Raised".x(fixture.ThenAMenuUpdatedEventIsNotRaised);
            "And the event CategoryCreated should not be Raised".x(fixture.ThenACategoryCreatedEventIsNotRaised);
        }

    }
}
