using FluentAssertions;
using Newtonsoft.Json;
using System;
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

        public MenuFixture()
        {
            //Add any fixture set up logic here
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
                throw new Exception($"Menu could not be created. API response: {await lastResponse.Content.ReadAsStringAsync()}");
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
            LastResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should be a successful request");
        }

        public void ThenTheMenuHasBeenDeleted()
        {
            LastResponse.StatusCode.Should().Be(HttpStatusCode.NoContent, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should be a successful request");
        }

        public async Task ThenICanViewTheMenuICreated()
        {
            LastResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should be a successful request");

            var responseMenu = JsonConvert.DeserializeObject<Menu>(await lastResponse.Content.ReadAsStringAsync());

            //compare the initial request sent to the API against the actual response

            responseMenu.name.Should().Be(createMenuRequest.name, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
            responseMenu.description.Should().Be(createMenuRequest.description, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
            responseMenu.enabled.Should().Be(createMenuRequest.enabled, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
        }

        public async Task ThenTheMenuIsUpdatedCorrectly()
        {
            LastResponse.StatusCode.Should().Be(HttpStatusCode.NoContent, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should be a successful request");

            var updatedResponse = await GetMenuById(existingMenuId);

            if(updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var updateMenuResponse = JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());

                updateMenuResponse.name.Should().Be(updateMenuRequest.name, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
                updateMenuResponse.description.Should().Be(updateMenuRequest.description, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
                updateMenuResponse.enabled.Should().Be(updateMenuRequest.enabled, $"because {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} should have created the menu");
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
