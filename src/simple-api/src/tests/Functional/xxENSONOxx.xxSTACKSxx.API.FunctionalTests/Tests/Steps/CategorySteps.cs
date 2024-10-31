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
/// These are the steps required for testing the Category endpoints
/// </summary>
public class CategorySteps
{
    private readonly string baseUrl;
    private HttpResponseMessage lastResponse;
    private string existingMenuId;
    private CategoryRequest createCategoryRequest;
    private CategoryRequest updateCategoryRequest;
    private string existingCategoryId;
    private const string categoryPath = "/category/";
    private readonly MenuSteps menuSteps = new MenuSteps();

    public CategorySteps()
    {
        var config = ConfigAccessor.GetApplicationConfiguration();
        baseUrl = config.BaseUrl;
    }

    #region Step Definitions

    #region Given

    public void GivenIHaveSpecfiedAFullCategory()
    {
        createCategoryRequest = new CategoryRequestBuilder()
            .SetDefaultValues("Yumido Test Category")
            .Build();
    }

    #endregion Given

    #region When

    public async Task<string> WhenICreateTheCategoryForAnExistingMenu()
    {
        existingMenuId = menuSteps.GivenAMenuAlreadyExists();

        lastResponse = await HttpRequestFactory.Post(baseUrl,
            $"{MenuSteps.menuPath}{existingMenuId}{categoryPath}", createCategoryRequest);

        existingCategoryId = JsonConvert
            .DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
            
        return existingCategoryId;
    }

    public async Task<string> CreateCategoryForSpecificMenu(String menuId)
    {
        lastResponse = await HttpRequestFactory.Post(baseUrl,
            $"{MenuSteps.menuPath}{menuId}{categoryPath}",
            new CategoryRequestBuilder()
                .SetDefaultValues("Yumido Test Category")
                .Build());

        existingCategoryId = JsonConvert
            .DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
        return existingCategoryId;
    }

    public async Task WhenISendAnUpdateCategoryRequest()
    {
        updateCategoryRequest = new CategoryRequestBuilder()
            .WithName("Updated Category Name")
            .WithDescription("Updated Category Description")
            .Build();
        String path = $"{MenuSteps.menuPath}{menuSteps.existingMenuId}{categoryPath}{existingCategoryId}";

        lastResponse = await HttpRequestFactory.Put(baseUrl, path, updateCategoryRequest);
    }

    public async Task WhenIDeleteTheCategory()
    {
        lastResponse = await HttpRequestFactory.Delete(baseUrl,
            $"{MenuSteps.menuPath}{existingMenuId}{categoryPath}{existingCategoryId}");
    }

    #endregion When

    #region Then

    public void ThenTheCategoryHasBeenCreated()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
    }

    public void ThenTheCategoryHasBeenDeleted()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

        //NOTE: add here validations other than the response StatusCode. For example, code trying to get the deleted record
        //var getCurrentMenu = await HttpRequestFactory.Get(baseUrl, $"{MenuSteps.menuPath}{existingMenuId}");
        //if (getCurrentMenu.StatusCode == HttpStatusCode.OK)
        //{
        //    var getCurrentMenuResponse =
        //        JsonConvert.DeserializeObject<Menu>(await getCurrentMenu.Content.ReadAsStringAsync());
        //    getCurrentMenuResponse.categories.ShouldBeEmpty();
        //}
    }

    public async Task ThenTheCategoryIsUpdatedCorrectly()
    {
        lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

        //NOTE: add here validations other than the response StatusCode. For example, code trying to get the deleted record
    }

    #endregion Then

    #endregion Step Definitions
}
