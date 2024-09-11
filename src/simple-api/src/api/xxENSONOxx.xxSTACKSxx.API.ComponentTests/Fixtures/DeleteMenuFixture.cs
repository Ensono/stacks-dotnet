using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.API.Authentication;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class DeleteMenuFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    public async Task WhenTheDeleteMenuIsSubmitted(Guid menuId)
    {
        LastResponse = await DeleteMenu(menuId);
    }
}
