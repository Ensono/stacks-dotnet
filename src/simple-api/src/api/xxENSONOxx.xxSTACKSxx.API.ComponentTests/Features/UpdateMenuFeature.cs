using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class UpdateMenuFeature
{
    [Theory, CustomAutoData]
    public void UpdateMenuValidMenuShouldSucceed(UpdateMenuFixture fixture)
    {
        this.Given(_ => fixture.WhenTheUpdateMenuIsSubmitted(Guid.NewGuid()), "When the menu is updated")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenANoContentResponseIsReturned(), "And the response code is NO CONTENT")
            .BDDfy();
    }
    
    [Theory, CustomAutoData]
    public void UpdateMenuInvalidMenuShouldNotSucceed(UpdateMenuFixture fixture)
    {
        this.Given(_ => fixture.WhenTheInvalidUpdateMenuIsSubmitted(Guid.NewGuid()), "When the invalid menu is updated")
            .Then(_ => fixture.ThenAnUnsuccessfulResponseIsReturned(), "Then an unsuccessful response is returned")
            .And(_ => fixture.ThenABadRequestResponseIsReturned(), "And the response code is BAD REQUEST")
            .BDDfy();
    }
}
