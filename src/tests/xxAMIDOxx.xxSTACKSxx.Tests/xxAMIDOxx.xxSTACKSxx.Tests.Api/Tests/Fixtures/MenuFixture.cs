using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
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
            catch (Exception ex)
            {
                throw new Exception($"Menu could not be created. innerException: {ex.InnerException}");
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

        public async Task ThenTheMenuHasBeenCreated()
        {
            Assert.True(lastResponse.StatusCode == HttpStatusCode.OK);

            var response = JsonConvert.DeserializeObject<CreateMenuResponse>(await lastResponse.Content.ReadAsStringAsync());
            Assert.True(Guid.TryParse(response.id, out Guid result));
        }

        public void ThenTheMenuHasBeenDeleted()
        {
            Assert.Equal(HttpStatusCode.NoContent, lastResponse.StatusCode);
        }

        public async Task ThenICanViewTheMenu()
        {
            Assert.True(lastResponse.StatusCode == HttpStatusCode.OK);

            var menu = JsonConvert.DeserializeObject<Menu>(await lastResponse.Content.ReadAsStringAsync());

            //compare the initial request sent to the API against the actual response
            Assert.Equal(createMenuRequest.name, menu.name);
            Assert.Equal(createMenuRequest.description, menu.description);
            Assert.Equal(createMenuRequest.enabled, menu.enabled);
        }

        public async Task ThenTheMenuIsUpdatedCorrectly()
        {
            Assert.Equal(HttpStatusCode.NoContent, lastResponse.StatusCode);

            var updatedResponse = await GetMenuById(existingMenuId);

            if(updatedResponse.StatusCode == HttpStatusCode.OK)
            {
                var menu = JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());

                Assert.Equal(updateMenuRequest.name, menu.name);
                Assert.Equal(updateMenuRequest.description, menu.description);
                Assert.Equal(updateMenuRequest.enabled, menu.enabled);
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
