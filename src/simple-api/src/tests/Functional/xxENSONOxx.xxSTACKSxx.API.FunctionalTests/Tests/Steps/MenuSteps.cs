using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Builders;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Models;
using xxENSONOxx.xxSTACKSxx.Tests.Api.Builders.Http;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

/// <summary>
/// These are the steps required for testing the Menu endpoints
/// </summary>
public class MenuSteps
{
    private MenuRequest createMenuRequest;
    private MenuRequest updateMenuRequest;
    private HttpResponseMessage lastResponse;
    public string existingMenuId;
    private readonly string baseUrl;
    public const string menuPath = "v1/menu/";

    public MenuSteps()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        baseUrl = config.BaseUrl;
    }

    #region Step Definitions

    #region Given

    public void GivenIHaveSpecfiedAFullMenu()
    {
        createMenuRequest = new MenuRequestBuilder()
            .SetDefaultValues("Yumido Test Menu")
            .Build();
    }

    #endregion Given

    #region When

    public async Task WhenISendAnUpdateMenuRequest()
    {
        updateMenuRequest = new MenuRequestBuilder()
            .WithName("Updated Menu Name")
            .WithDescription("Updated Description")
            .SetEnabled(true)
            .Build();

        lastResponse = await HttpRequestFactory.Put(baseUrl, $"{menuPath}{existingMenuId}", updateMenuRequest);
    }

    public string GivenAMenuAlreadyExists()
    {
        //NOTE: create here with post and return id. Any id will work in this case, as the API is not persisting data
        existingMenuId = Guid.NewGuid().ToString();
        return existingMenuId;
    }

    public async Task WhenICreateTheMenu()
    {
        lastResponse = await HttpRequestFactory.Post(baseUrl, menuPath, createMenuRequest);
    }

    public async Task WhenIDeleteAMenu()
    {
        lastResponse = await HttpRequestFactory.Delete(baseUrl, $"{menuPath}{existingMenuId}");
    }

    public async Task WhenIGetAMenu()
    {
        lastResponse = await HttpRequestFactory.Get(baseUrl, $"{menuPath}{existingMenuId}");
    }

    #endregion When

    #region Then

    public void ThenTheMenuHasBeenCreated()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
    }

    public void ThenTheMenuHasBeenDeleted()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
    }

    public async Task ThenICanReadTheMenuReturned()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

        var responseMenu = JsonConvert.DeserializeObject<Menu>(await lastResponse.Content.ReadAsStringAsync());
        responseMenu.ShouldNotBeNull();

        //NOTE: compare the initial request sent to the API against the actual response
        //Not doable here because the response given in the API is not related to the request, currently
    }

    public async Task ThenTheMenuIsUpdatedCorrectly()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

        //NOTE: compare the initial request sent to the API against the actual response
    }

    #endregion Then

    #endregion Step Definitions
}
