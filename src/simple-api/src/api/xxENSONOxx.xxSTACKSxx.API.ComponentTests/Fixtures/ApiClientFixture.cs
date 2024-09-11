using System.Net;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

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
        collection.AddSingleton<IOptions<JwtBearerAuthenticationConfiguration>>(_ => jwtBearerAuthenticationOptions);
    }

    /// <summary>
    /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
    /// </summary>
    /// <param name="menu">Menu being created</param>
    protected async Task<HttpResponseMessage> CreateMenu(CreateMenuRequest menu)
    {
        return await SendAsync(HttpMethod.Post, "/v1/menu", menu);
    }
    
    /// <summary>
    /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
    /// </summary>
    /// <param name="menu">Menu being created</param>
    protected async Task<HttpResponseMessage> UpdateMenu(Guid menuId, UpdateMenuRequest menu)
    {
        return await SendAsync(HttpMethod.Put, $"/v1/menu/{menuId}", menu);
    }
    
    /// <summary>
    /// Send a POST Http request to the API DeleteMenu endpoint passing the Id of the menu being deleted
    /// </summary>
    /// <param name="menuId">id of the menu being deleted</param>
    protected async Task<HttpResponseMessage> DeleteMenu(Guid menuId)
    {
        return await SendAsync(HttpMethod.Delete, $"/v1/menu/{menuId}");
    }
    
    /// <summary>
    /// Send a GET Http request to the API Menu endpoint passing the Id of the menu being retrieved
    /// </summary>
    /// <param name="menuId">id of the menu being retrieved</param>
    protected async Task<HttpResponseMessage> GetMenu(Guid menuId)
    {
        return await SendAsync(HttpMethod.Get, $"/v1/menu/{menuId}");
    }
    
    internal void ThenASuccessfulResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeTrue();
    }
    
    internal void ThenAnUnsuccessfulResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeFalse();
    }

    internal void ThenACreatedResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    internal void ThenABadRequestResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    internal void ThenANoContentResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
