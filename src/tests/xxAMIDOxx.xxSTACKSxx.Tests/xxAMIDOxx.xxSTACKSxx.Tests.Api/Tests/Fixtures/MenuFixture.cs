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
            var requestObject = new MenuRequestBuilder()
                .WithName("Updated Menu Name")
                .WithDescription("Updated Description")
                .SetEnabled(true)
                .Build();

            lastResponse = await UpdateMenu(requestObject, existingMenuId);
        }

        public async Task WhenICreateTheMenu()
        {
            //Todo: Add authentication to requests (bearer xyz)
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

        public void ThenTheMenuHasBeenSuccessfullyPublished()
        {
            Assert.True(lastResponse.StatusCode == System.Net.HttpStatusCode.OK);
            //Check the menu is in the DB
        }

        public void ThenTheMenuHasBeenDeleted()
        {
            Assert.Equal(HttpStatusCode.NoContent, lastResponse.StatusCode);

            //ToDo: Assert the DB
        }

        public void ThenICanViewTheMenu()
        {
            Assert.True(lastResponse.StatusCode == HttpStatusCode.OK);
        }

        public void ThenTheMenuIsUpdatedCorrectly()
        {
            Assert.Equal(HttpStatusCode.NoContent, lastResponse.StatusCode);

            //ToDo: Check menu is in Database with updated values
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
