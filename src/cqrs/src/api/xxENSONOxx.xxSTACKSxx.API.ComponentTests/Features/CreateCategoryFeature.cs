using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class CreateCategoryFeature
{
    [Theory, CustomAutoData]
    public void CreateCategoryShouldSucceed(CreateCategoryFixture fixture)
    {
        this.Given(_ => fixture.GivenAnExistingMenu(), "And an existing menu")
                .And(_ => fixture.GivenTheMenuBelongsToUserRestaurant(), "And the menu belongs to the user restaurant")
                .And(_ => fixture.GivenTheCategoryDoesNotExist(), "And the category being created does not exist in the menu")
            .When(_ => fixture.WhenTheCategoryIsSubmitted(), "When a new category is submitted")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
                .And(_ => fixture.ThenMenuIsLoadedFromStorage(), "And the menu is loaded from the storage")
                .And(_ => fixture.ThenTheResourceCreatedResponseIsReturned(), "And the id of the new category is returned")
                .And(_ => fixture.ThenTheCategoryIsAddedToMenu(), "And the category is added to the menu")
                .And(_ => fixture.ThenTheMenuShouldBePersisted(), "And the menu is persisted to the storage")
                .And(_ => fixture.ThenAMenuUpdatedEventIsRaised(), "And the event MenuUpdate is Raised")
                .And(_ => fixture.ThenACategoryCreatedEventIsRaised(), "And the event CategoryCreated is Raised.")
            .BDDfy();
    }


    [Theory, CustomAutoData]
    public void CreateCategoryShouldFailWhenMenuDoesNotExist(CreateCategoryFixture fixture)
    {
        this.Given(_ => fixture.GivenAMenuDoesNotExist(), "A menu does not exist")
            .When(_ => fixture.WhenTheCategoryIsSubmitted(), "When a new category is submitted")
            .Then(_ => fixture.ThenAFailureResponseIsReturned(), "Then a failure response is returned")
            .And(_ => fixture.ThenANotFoundResponseIsReturned(), "And the response code is NotFound")
            .And(_ => fixture.ThenTheMenuShouldNotBePersisted(), "And the menu is not persisted to the storage")
            .And(_ => fixture.ThenAMenuUpdatedEventIsNotRaised(), "And the event MenuUpdate should NOT be raised")
            .And(_ => fixture.ThenACategoryCreatedEventIsNotRaised(),
                "And the event CategoryCreated should not be Raised")
            .BDDfy();
    }


    [Theory, CustomAutoData]
    public void CreateCategoryShouldFailWhenCategoryAlreadyExists(CreateCategoryFixture fixture)
    {
        this.Given(_ => fixture.GivenAnExistingMenu(), "And an existing menu")
            .And(_ => fixture.GivenTheMenuBelongsToUserRestaurant(), "And the menu belongs to the user restaurant")
            .And(_ => fixture.GivenTheCategoryAlreadyExist(),
                "And the category being created already exist in the menu")
            .When(_ => fixture.WhenTheCategoryIsSubmitted(), "When a new category is submitted")
            .Then(_ => fixture.ThenAFailureResponseIsReturned(), "Then a failure response is returned")
            .And(_ => fixture.ThenAConflictResponseIsReturned(), "And the response code is Conflict")
            .And(_ => fixture.ThenTheMenuShouldNotBePersisted(), "And the menu is NOT persisted to the storage")
            .And(_ => fixture.ThenAMenuUpdatedEventIsNotRaised(), "And the event MenuUpdate should NOT be raised")
            .And(_ => fixture.ThenACategoryCreatedEventIsNotRaised(), "And the event CategoryCreated is NOT Raised")
            .BDDfy();
    }
}
