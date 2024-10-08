using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.API.Models.Responses;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

/// <summary>
/// ApiClientFixture handles the communication with TestServer
/// and httpClient handling
/// </summary>
public abstract class ApiClientFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiFixture<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"JwtBearerAuthentication:AllowExpiredTokens", jwtBearerAuthenticationOptions.Value.AllowExpiredTokens.ToString().ToLowerInvariant()},
                {"JwtBearerAuthentication:Audience", jwtBearerAuthenticationOptions.Value.Audience},
                {"JwtBearerAuthentication:Authority", jwtBearerAuthenticationOptions.Value.Authority},
                {"JwtBearerAuthentication:Enabled", jwtBearerAuthenticationOptions.Value.Enabled.ToString().ToLowerInvariant()},
                {"JwtBearerAuthentication:OpenApi:AuthorizationUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.AuthorizationUrl!},
                {"JwtBearerAuthentication:OpenApi:ClientId", jwtBearerAuthenticationOptions.Value.OpenApi?.ClientId!},
                {"JwtBearerAuthentication:OpenApi:TokenUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.TokenUrl!},
                {"JwtBearerAuthentication:UseStubbedBackchannelHandler", jwtBearerAuthenticationOptions.Value.UseStubbedBackchannelHandler.ToString().ToLowerInvariant()},
            }!);
        });
    }

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        collection.AddSingleton(_ => jwtBearerAuthenticationOptions);
    }
    
    /// <summary>
    /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
    /// </summary>
    /// <param name="menu">Menu being created</param>
    protected async Task CreateMenu(CreateMenuRequest menu)
    {
        await SendAsync(HttpMethod.Post, "/v1/menu", menu);
    }
    
    /// <summary>
    /// Send a POST Http request to the API DeleteMenu endpoint passing the menuId being deleted
    /// </summary>
    /// <param name="menuId">Menu being deleted</param>
    protected async Task DeleteMenu(string menuId)
    {
        await SendAsync(HttpMethod.Delete, $"/v1/menu/{menuId}");
    }

    /// <summary>
    /// Send a POST Http request to the API CreateCategory endpoint passing the menu id and category being created
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="category">Category being created</param>
    protected async Task CreateCategory(Guid menuId, CreateCategoryRequest category)
    {
        await SendAsync(HttpMethod.Post, $"/v1/menu/{menuId}/category", category);
    }

    internal void ThenASuccessfulResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    internal void ThenAFailureResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeFalse();
    }

    internal void ThenACreatedResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    internal void ThenANotFoundResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    internal void ThenAConflictResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    internal async Task ThenTheResourceCreatedResponseIsReturned()
    {
        var responseObject = await GetResponseObject<ResourceCreatedResponse>();

        responseObject.ShouldNotBeNull();
        responseObject.Id.ShouldNotBe(Guid.Empty);
    }
}
