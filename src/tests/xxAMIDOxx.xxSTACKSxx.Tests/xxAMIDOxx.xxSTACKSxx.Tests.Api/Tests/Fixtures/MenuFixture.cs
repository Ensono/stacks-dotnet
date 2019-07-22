using Newtonsoft.Json;
using Shouldly;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures
{
    /// <summary>
    /// Fixtures contain all test step definitions for the story related to it (I.e. Menu)
    /// This also contains the step up and tear down for the fixture
    /// </summary>
    public class MenuFixture : ClientFixture, IDisposable
    {
        private MenuRequest createMenuRequest;
        private MenuRequest updateMenuRequest;
        private HttpResponseMessage lastResponse;
        public string existingMenuId;

        public MenuFixture(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            //Add any fixture set up logic here
            Debug.WriteLine("Menu Fixture constructor");
        }

        #region Step Definitions
        public async Task GivenAMenuAlreadyExists()
        {
            createMenuRequest = new MenuRequestBuilder()
                .SetDefaultValues("Yumido Test Menu")
                .Build();

            try
            {
                lastResponse = await CreateMenu(createMenuRequest);

                if(lastResponse.StatusCode == HttpStatusCode.OK)
                {
                    existingMenuId = JsonConvert.DeserializeObject<CreateMenuResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception($"Menu could not be created. API response: {lastResponse.Content.ToString()}");
            }
        }

        public void GivenIHaveSpecfiedAFullMenu()
        {
            createMenuRequest = new MenuRequestBuilder()
                .SetDefaultValues("Yumido Test Menu")
                .Build();
        }

        public async Task WhenISendAnUpdateMenuRequest()
        {
            updateMenuRequest = new MenuRequestBuilder()
                .WithName("Updated Menu Name")
                .WithDescription("Updated Description")
                .SetEnabled(true)
                .Build();

            lastResponse = await UpdateMenu(updateMenuRequest, existingMenuId);
        }

        public async Task WhenICreateTheMenu()
        {
            lastResponse = await CreateMenu(createMenuRequest);
        }

        public async Task WhenIDeleteAMenu()
        {
            lastResponse = await DeleteMenu(existingMenuId);
        }

        public async Task WhenIGetAMenu()
        {
            lastResponse = await GetMenuById(existingMenuId);
        }

        public void ThenTheMenuHasBeenCreated()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK, 
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
        }

        public void ThenTheMenuHasBeenDeleted()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent, 
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
        }

        public async Task ThenICanReadTheMenuReturned()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK, $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var responseMenu = JsonConvert.DeserializeObject<Menu>(await lastResponse.Content.ReadAsStringAsync());

            //compare the initial request sent to the API against the actual response
            responseMenu.name.ShouldBe(createMenuRequest.name, 
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

            responseMenu.description.ShouldBe(createMenuRequest.description, 
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

            responseMenu.enabled.ShouldBe(createMenuRequest.enabled, 
                $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
        }

        public async Task ThenTheMenuIsUpdatedCorrectly()
        {
            lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent, 
                $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

            var updatedResponse = await GetMenuById(existingMenuId);

            if(updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updateMenuResponse = JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());

                updateMenuResponse.name.ShouldBe(updateMenuRequest.name, 
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

                updateMenuResponse.description.ShouldBe(updateMenuRequest.description, 
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

                updateMenuResponse.enabled.ShouldBe(updateMenuRequest.enabled, 
                    $"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
            }
            else
            {
                //throw exception rather than use assertions if the GET request fails as GET is not the subject of the test
                //Assertions should only be used on the subject of the test
                throw new Exception($"Could not retrieve the updated menu using GET /menu/{existingMenuId}");
            }
        }
        #endregion Step Definitions

        #region Dispose/teardown logic
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                //Fixture teardown steps go here
            }
        }
        #endregion
    }
}
