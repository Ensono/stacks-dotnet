using System.Net.Http;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Configuration;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures
{
    public class ClientFixture : ApiFixture
    {
        public ConfigModel config;

        //Create a new HttpClient using the configuration base url
        public override HttpClient BuildHttpClient()
        {
            config = ConfigAccessor.GetApplicationConfiguration();
            return new HttpClient()
            {
                BaseAddress = new System.Uri(config.BaseUrl)
            };
        }
        
        //API clients
        public async Task<HttpResponseMessage> CreateMenu(MenuRequest request)
        {
            return await SendAsync(HttpMethod.Post, "v1/menu/", request);
        }

        public async Task<HttpResponseMessage> GetMenuById(string id)
        {
            return await SendAsync(HttpMethod.Get, $"v1/menu/{id}");
        }

        public async Task<HttpResponseMessage> UpdateMenu(MenuRequest request, string id)
        {
            return await SendAsync(HttpMethod.Put, $"v1/menu/{id}", request);
        }

        public async Task<HttpResponseMessage> DeleteMenu(string id)
        {
            return await SendAsync(HttpMethod.Delete, $"v1/menu/{id}");
        }

        public void GivenAUser()
        {
            //Set request headers/create user token
        }

        public void GivenAnAdmin()
        {
            //Set request headers/create admin token
        }
    }
}
