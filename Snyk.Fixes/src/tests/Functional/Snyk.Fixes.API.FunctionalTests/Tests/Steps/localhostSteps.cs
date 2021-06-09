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
    /// These are the steps required for testing the localhost endpoints
    /// </summary>
    public class localhostSteps
    {
        private localhostRequest createlocalhostRequest;
        private localhostRequest updatelocalhostRequest;
        private HttpResponseMessage lastResponse;
        public string existinglocalhostId;
        private readonly string baseUrl;
        public const string localhostPath = "v1/localhost/";

        public localhostSteps()
        {
            var config = ConfigAccessor.GetApplicationConfiguration();
            baseUrl = config.BaseUrl;
        }

        #region Step Definitions

        #region Given

        public async Task<string> GivenAlocalhostAlreadyExists()
        {
            createlocalhostRequest = new localhostRequestBuilder()
                .SetDefaultValues("Yumido Test localhost")
                .Build();

            try
            {
                lastResponse = await HttpRequestFactory.Post(baseUrl, localhostPath, createlocalhostRequest);

                if (lastResponse.StatusCode == HttpStatusCode.Created)
                {
                    existinglocalhostId = JsonConvert
                        .DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception(
                    $"localhost could not be created. API response: {await lastResponse.Content.ReadAsStringAsync()}");
            }

            return existinglocalhostId;
        }

        public void GivenIHaveSpecfiedAFulllocalhost()
        {
            createlocalhostRequest = new localhostRequestBuilder()
                .SetDefaultValues("Yumido Test localhost")
                .Build();
        }

        #endregion Given

        #region When

        public async Task WhenISendAnUpdatelocalhostRequest()
        {
            updatelocalhostRequest = new localhostRequestBuilder()
                .WithName("Updated localhost Name")
                .WithDescription("Updated Description")
                .SetEnabled(true)
                .Build();

            lastResponse = await HttpRequestFactory.Put(baseUrl, $"{localhostPath}{existinglocalhostId}", updatelocalhostRequest);
        }

        public async Task WhenICreateThelocalhost()
        {
            lastResponse = await HttpRequestFactory.Post(baseUrl, localhostPath, createlocalhostRequest);
        }

        public async Task WhenIDeleteAlocalhost()
        {
            lastResponse = await HttpRequestFactory.Delete(baseUrl, $"{localhostPath}{existinglocalhostId}");
        }

        public async Task WhenIGetAlocalhost()
        {
            lastResponse = await HttpRequestFactory.Get(baseUrl, $"{localhostPath}{existinglocalhostId}");
        }

        #endregion When

        #region Then

        public void ThenThelocalhostHasBeenCreated()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
        }

        public void ThenThelocalhostHasBeenDeleted()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
        }

        public async Task ThenICanReadThelocalhostReturned()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var responselocalhost = JsonConvert.DeserializeObject<localhost>(await lastResponse.Content.ReadAsStringAsync());

            //compare the initial request sent to the API against the actual response
            responselocalhost.name.ShouldBe(createlocalhostRequest.name,
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

            responselocalhost.description.ShouldBe(createlocalhostRequest.description,
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

            responselocalhost.enabled.ShouldBe(createlocalhostRequest.enabled,
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");
        }

        public async Task ThenThelocalhostIsUpdatedCorrectly()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{localhostPath}{existinglocalhostId}");

            if (updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updatelocalhostResponse =
                    JsonConvert.DeserializeObject<localhost>(await updatedResponse.Content.ReadAsStringAsync());

                updatelocalhostResponse.name.ShouldBe(updatelocalhostRequest.name,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

                updatelocalhostResponse.description.ShouldBe(updatelocalhostRequest.description,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");

                updatelocalhostResponse.enabled.ShouldBe(updatelocalhostRequest.enabled,
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the localhost as expected");
            }
            else
            {
                //throw exception rather than use assertions if the GET request fails as GET is not the subject of the test
                //Assertions should only be used on the subject of the test
                throw new Exception($"Could not retrieve the updated localhost using GET /localhost/{existinglocalhostId}");
            }
        }

        #endregion Then

        #endregion Step Definitions
    }
}