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
    //Fixture set up and tear down
    //Inherits IDisposable as this is where teardown will take place
    public class MenuFixture : ClientFixture, IDisposable
    {
        private MenuRequest menuRequest;
        private HttpResponseMessage lastResponse;
        private MenuRequest updateMenuRequest;
        public string existingMenuId;

        public async Task GivenAMenuAlreadyExists()
        {
            menuRequest = new MenuRequestBuilder()
                .SetDefaultValues("Yumido Test Menu")
                .Build();

            try
            {
                lastResponse = await CreateMenu(menuRequest);

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
            menuRequest = new MenuRequestBuilder()
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
            lastResponse = await CreateMenu(menuRequest);
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

            Assert.Equal(menuRequest.name, menu.name);
            Assert.Equal(menuRequest.description, menu.description);
            Assert.Equal(menuRequest.enabled, menu.enabled);
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
                //throw exception rather than use assertions if the GET request fails as it is not the subject of the test
                //Assertions should only be used on the subject of the test
                throw new Exception($"Could not retrieve the updated menu using GET /menu/{existingMenuId}");
            }

        }

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
                //I.e. Delete test users from DB
            }
        }
        #endregion
    }
}
