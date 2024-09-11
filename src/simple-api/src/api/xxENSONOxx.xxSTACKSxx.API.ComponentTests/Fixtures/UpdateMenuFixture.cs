using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class UpdateMenuFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    public async Task WhenTheUpdateMenuIsSubmitted(Guid menuId)
    {
        LastResponse = await UpdateMenu(menuId, new UpdateMenuRequest
        {
            Name = "some name",
            Description = "some description",
            Enabled = true,
            RestaurantId = Guid.NewGuid()
        });
    }
    
    public async Task WhenTheInvalidUpdateMenuIsSubmitted(Guid menuId)
    {
        LastResponse = await UpdateMenu(menuId, new UpdateMenuRequest());
    }
}
