using Microsoft.Extensions.Options;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Responses;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class GetMenuFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    public async Task WhenTheGetMenuIsSubmitted(Guid menuId)
    {
        LastResponse = await GetMenu(menuId);
    }

    public async Task ThenTheMenuIsReturned()
    {
        var menu = await LastResponse.Content.ReadAsAsync<Menu>();
        menu.ShouldNotBeNull();
        menu.ShouldBeOfType<Menu>();

        menu.Description.ShouldNotBeNull();
        menu.Enabled.ShouldBeOfType<bool>();
        menu.Id.ShouldNotBeNull();
        menu.Categories.ShouldNotBeNull();
        menu.Categories.ShouldNotBeEmpty();
    }
}
