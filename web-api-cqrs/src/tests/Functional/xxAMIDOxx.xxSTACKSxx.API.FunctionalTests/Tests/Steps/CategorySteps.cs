using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Configuration;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders.Http;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps
{
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
            existingMenuId = await menuSteps.GivenAMenuAlreadyExists();

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

        public async Task ThenTheCategoryHasBeenDeleted()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var getCurrentMenu = await HttpRequestFactory.Get(baseUrl, $"{MenuSteps.menuPath}{existingMenuId}");
            if (getCurrentMenu.StatusCode == HttpStatusCode.OK)
            {
                var getCurrentMenuResponse =
                    JsonConvert.DeserializeObject<Menu>(await getCurrentMenu.Content.ReadAsStringAsync());

                getCurrentMenuResponse.categories.ShouldBeEmpty();
            }
        }

        public async Task ThenTheCategoryIsUpdatedCorrectly()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{MenuSteps.menuPath}{existingMenuId}");

            if (updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updateCategoryResponse =
                    JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());


                updateCategoryResponse.categories[0].name.ShouldBe(updateCategoryRequest.name,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

                updateCategoryResponse.categories[0].description.ShouldBe(updateCategoryRequest.description,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
            }
            else
            {
                throw new Exception($"Could not retrieve the updated menu using GET /menu/{existingMenuId}");
            }
        }

        #endregion Then

        #endregion Step Definitions
    }
}