using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Features;

[Trait("TestType", "ComponentTests")]
public class DeleteMenuFeature
{
    [Theory, CustomAutoData]
    public void DeleteMenuValidMenuIdShouldSucceed(DeleteMenuFixture fixture)
    {
        this.Given(_ => fixture.WhenTheDeleteMenuIsSubmitted(Guid.NewGuid()), "When the menu is deleted")
            .Then(_ => fixture.ThenASuccessfulResponseIsReturned(), "Then a successful response is returned")
            .And(_ => fixture.ThenANoContentResponseIsReturned(), "And the response code is NO CONTENT")
            .BDDfy();
    }
}
