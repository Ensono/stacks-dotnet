using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Snyk.Fixes.API.FunctionalTests.Builders;
using Snyk.Fixes.API.FunctionalTests.Configuration;
using Snyk.Fixes.API.FunctionalTests.Models;
using Snyk.Fixes.Tests.Api.Builders.Http;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Steps
{
    /// <summary>
    /// These are the steps required for testing the Category endpoints
    /// </summary>
    public class CategorySteps
    {
        private readonly string baseUrl;
        private HttpResponseMessage lastResponse;
        private string existinglocalhostId;
        private CategoryRequest createCategoryRequest;
        private CategoryRequest updateCategoryRequest;
        private string existingCategoryId;
        private const string categoryPath = "/category/";
        private readonly localhostSteps localhostSteps = new localhostSteps();

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

        public async Task<string> WhenICreateTheCategoryForAnExistinglocalhost()
        {
            existinglocalhostId = await localhostSteps.GivenAlocalhostAlreadyExists();

            lastResponse = await HttpRequestFactory.Post(baseUrl,
                $"{localhostSteps.localhostPath}{existinglocalhostId}{categoryPath}", createCategoryRequest);

            existingCategoryId = JsonConvert
                .DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
            return existingCategoryId;
        }

        public async Task<string> CreateCategoryForSpecificlocalhost(String localhostId)
        {
            lastResponse = await HttpRequestFactory.Post(baseUrl,
                $"{localhostSteps.localhostPath}{localhostId}{categoryPath}",
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
            String path = $"{localhostSteps.localhostPath}{localhostSteps.existinglocalhostId}{categoryPath}{existingCategoryId}";

            lastResponse = await HttpRequestFactory.Put(baseUrl, path, updateCategoryRequest);
        }

        public async Task WhenIDeleteTheCategory()
        {
            lastResponse = await HttpRequestFactory.Delete(baseUrl,
                $"{localhostSteps.localhostPath}{existinglocalhostId}{categoryPath}{existingCategoryId}");
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

            var getCurrentlocalhost = await HttpRequestFactory.Get(baseUrl, $"{localhostSteps.localhostPath}{existinglocalhostId}");
            if (getCurrentlocalhost.StatusCode == HttpStatusCode.OK)
            {
                var getCurrentlocalhostResponse =
                    JsonConvert.DeserializeObject<localhost>(await getCurrentlocalhost.Content.ReadAsStringAsync());

                getCurrentlocalhostResponse.categories.ShouldBeEmpty();
            }
        }

        public async Task ThenTheCategoryIsUpdatedCorrectly()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{localhostSteps.localhostPath}{existinglocalhostId}");

            if (updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updateCategoryResponse =
                    JsonConvert.DeserializeObject<localhost>(await updatedResponse.Content.ReadAsStringAsync());


                updateCategoryResponse.categories[0].name.ShouldBe(updateCategoryRequest.name,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

                updateCategoryResponse.categories[0].description.ShouldBe(updateCategoryRequest.description,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");
            }
            else
            {
                throw new Exception($"Could not retrieve the updated localhost using GET /localhost/{existinglocalhostId}");
            }
        }

        #endregion Then

        #endregion Step Definitions
    }
}