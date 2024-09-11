using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class GetMenuByIdFeature
{
    [Theory, CustomAutoData]
    public void GetMenuValidMenuIdShouldSucceed(GetMenuFixture fixture)
    {
        this.Given(_ => fixture.WhenTheGetMenuIsSubmitted(Guid.NewGuid()), "When the menu is retrieved")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenTheMenuIsReturned(), "And the menu is returned")
            .BDDfy();
    }
}
