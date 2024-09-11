using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class CreateMenuFeature
{
    [Theory, CustomAutoData]
    public void CreateMenuAsAdminForValidMenuShouldSucceed(CreateMenuFixture fixture)
    {
        this.Given(_ => fixture.WhenTheMenuCreationIsSubmitted(), "When the menu is submitted")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenACreatedResponseIsReturned(), "And the response code is CREATED")
            .BDDfy();
    }
}
