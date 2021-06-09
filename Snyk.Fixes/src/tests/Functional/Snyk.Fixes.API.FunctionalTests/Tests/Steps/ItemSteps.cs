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
    public class ItemSteps
    {
        private readonly localhostSteps localhostSteps = new localhostSteps();
        private readonly CategorySteps categorySteps = new CategorySteps();
        private readonly string baseUrl;
        private HttpResponseMessage lastResponse;
        private string existinglocalhostId;
        private string existingCategoryId;
        private string existingItemId;
        private localhostItemRequest createItemRequest;
        private localhostItemRequest updateItemRequest;
        private const string categoryPath = "/category/";
        private const string itemPath = "/items/";

        public ItemSteps()
        {
            var config = ConfigAccessor.GetApplicationConfiguration();
            baseUrl = config.BaseUrl;
        }

        #region Step Definitions

        #region Given

        public void GivenIHaveSpecfiedAFullItem()
        {
            createItemRequest = new localhostItemBuilder()
                .SetDefaultValues("Yumido Test Item")
                .Build();
        }

        #endregion Given

        #region When

        public async Task WhenISendAnUpdateItemRequest()
        {
            updateItemRequest = new localhostItemBuilder()
                .WithName("Updated item name")
                .WithDescription("Updated item description")
                .WithPrice(4.5)
                .WithAvailablity(true)
                .Build();
            String path =
                $"{localhostSteps.localhostPath}{existinglocalhostId}{categoryPath}{existingCategoryId}{itemPath}{existingItemId}";

            lastResponse = await HttpRequestFactory.Put(baseUrl, path, updateItemRequest);
        }

        public async Task WhenICreateTheItemForAnExistinglocalhostAndCategory()
        {
            existinglocalhostId = await localhostSteps.GivenAlocalhostAlreadyExists();
            existingCategoryId = await categorySteps.CreateCategoryForSpecificlocalhost(existinglocalhostId);

            lastResponse = await HttpRequestFactory.Post(baseUrl,
                $"{localhostSteps.localhostPath}{existinglocalhostId}{categoryPath}{existingCategoryId}{itemPath}", createItemRequest);
            existingItemId = JsonConvert
                .DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
        }

        public async Task WhenIDeleteTheItem()
        {
            lastResponse = await HttpRequestFactory.Delete(baseUrl,
                $"{localhostSteps.localhostPath}{existinglocalhostId}{categoryPath}{existingCategoryId}{itemPath}{existingItemId}");
        }

        #endregion When

        #region Then

        public void ThenTheItemHasBeenCreated()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
        }

        public async Task ThenTheItemHasBeenDeleted()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var getCurrentlocalhost = await HttpRequestFactory.Get(baseUrl, $"{localhostSteps.localhostPath}{existinglocalhostId}");
            if (getCurrentlocalhost.StatusCode == HttpStatusCode.OK)
            {
                var getCurrentlocalhostResponse =
                    JsonConvert.DeserializeObject<localhost>(await getCurrentlocalhost.Content.ReadAsStringAsync());

                getCurrentlocalhostResponse.categories[0].items.ShouldBeEmpty();
            }
        }

        public async Task ThenTheItemIsUpdatedCorrectly()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{localhostSteps.localhostPath}{existinglocalhostId}");

            if (updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updateItemResponse =
                    JsonConvert.DeserializeObject<localhost>(await updatedResponse.Content.ReadAsStringAsync());


                updateItemResponse.categories[0].items[0].name.ShouldBe(updateItemRequest.name,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

                updateItemResponse.categories[0].items[0].description.ShouldBe(updateItemRequest.description,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

                updateItemResponse.categories[0].items[0].price.ShouldBe(updateItemRequest.price.ToString(),
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");
                updateItemResponse.categories[0].items[0].available.ShouldBeTrue();
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