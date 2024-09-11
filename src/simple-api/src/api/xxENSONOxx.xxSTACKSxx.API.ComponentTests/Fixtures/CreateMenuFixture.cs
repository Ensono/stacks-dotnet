using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class CreateMenuFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions) : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    public async Task WhenTheMenuCreationIsSubmitted()
    {
        LastResponse = await CreateMenu(new CreateMenuRequest
        {
            Name = "some name",
            Description = "some description",
            Enabled = true,
            TenantId = Guid.NewGuid()
        });
    }
}
