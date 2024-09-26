using TestStack.BDDfy;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class CreateMenuFeature
{
    [Theory, CustomAutoData]
    public void CreateMenuForValidMenuShouldSucceed(CreateMenuFixture fixture)
    {
        this.Given(_ => fixture.GivenAMenuDoesNotExist(), "And the menu does not does not exist")
            .When(_ => fixture.WhenTheMenuCreationIsSubmitted(), "When the menu is submitted")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenACreatedResponseIsReturned(), "And the response code is CREATED")
            .And(_ => fixture.ThenTheResourceCreatedResponseIsReturned(), "And the id of the new menu is returned")
            .And(_ => fixture.ThenTheMenuIsSubmittedToDatabase(),
                "And the menu data is submitted correctly to the database")
            .And(_ => fixture.ThenAMenuCreatedEventIsRaised(),
                $"And an event of type {nameof(MenuCreatedEvent)} is raised")
            .BDDfy();
    }


    [Theory, CustomAutoData]
    public void CreateMenuForInvalidMenuShouldFail(CreateMenuFixture fixture)
    {
        this.Given(_ => fixture.GivenAInvalidMenu(), "And a valid menu being submitted")
            .And(_ => fixture.GivenAMenuDoesNotExist(), "And the menu does not does not exist")
            .When(_ => fixture.WhenTheMenuCreationIsSubmitted(), "When the menu is submitted")
            .Then(_ => fixture.ThenAFailureResponseIsReturned(), "Then a failure response is returned")
            .And(_ => fixture.ThenTheMenuIsNotSubmittedToDatabase(), "And the menu is not submitted to the database")
            .And(_ => fixture.ThenAMenuCreatedEventIsNotRaised(),
                $"And an event of type {nameof(MenuCreatedEvent)} should not be raised")
            .BDDfy();
    }
}
