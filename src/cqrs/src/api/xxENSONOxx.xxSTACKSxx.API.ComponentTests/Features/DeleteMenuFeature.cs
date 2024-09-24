using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class DeleteMenuFeature
{
    [Theory, CustomAutoData]
    public void DeleteMenuForExistingMenuShouldSucceed(DeleteMenuFixture fixture)
    {
        this.Given(_ => fixture.GivenAMenuExists(), "And the menu exists")
            .When(_ => fixture.WhenTheMenuDeletionIsSubmitted(), "When the menu deletion is submitted")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenTheMenuIsRemovedFromDatabase(), "And the menu is removed from the database")
            .And(_ => fixture.ThenAMenuDeletedEventIsRaised(), $"And an event of type {nameof(MenuDeletedEvent)} is raised")
            .BDDfy();
    }

    [Theory, CustomAutoData]
    public void DeleteMenuForNonExistingMenuShouldFail(DeleteMenuFixture fixture)
    {
        this.Given(_ => fixture.GivenAMenuDoesNotExist(), "And the menu does not exist")
            .When(_ => fixture.WhenTheMenuDeletionIsSubmitted(), "When the menu deletion is submitted")
            .Then(_ => fixture.ThenAFailureResponseIsReturned(), "Then a failure response is returned")
            .And(_ => fixture.ThenNoMenuIsRemovedFromDatabase(), "And no menu is removed from the database")
            .And(_ => fixture.ThenAMenuDeletedEventIsNotRaised(), $"And an event of type {nameof(MenuDeletedEvent)} should not be raised")
            .BDDfy();
    }
}
