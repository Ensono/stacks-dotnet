using System.Net.Http;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.API.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures
{
    /// <summary>
    /// ApiClientFixture handles the communication with TestServer
    /// and httpClient handling
    /// </summary>
    public abstract class ApiClientFixture : ApiFixture<Startup>
    {
        /// <summary>
        /// When a feature has same behaviour for multiple roles
        /// We could use the same theory with roles passed as parameter
        /// </summary>
        /// <param name="role"></param>
        public void AsRole(string role)
        {
            switch (role.ToLower())
            {
                case "admin":
                    AsAdmin();
                    break;
                case "employee":
                    AsEmployee();
                    break;
                case "customer":
                    AsCustomer();
                    break;
                default:
                    AsUnauthenticatedUser();
                    break;
            }
        }

        public void AsAdmin()
        {
            base.httpClient.AsAdmin();
        }

        public void AsEmployee()
        {
            base.httpClient.AsEmployee();
        }

        public void AsCustomer()
        {
            base.httpClient.AsCustomer();
        }

        public void AsUnauthenticatedUser()
        {
            base.httpClient.AsUnauthenticatedUser();
        }

        public async Task<HttpResponseMessage> CreateMenu(CreateOrUpdateMenu menu)
        {
            //LastResponse = await base.httpClient.PostAsync("/v1/menu", CreateHttpContent<CreateOrUpdateMenu>(menu));
            return await SendAsync<CreateOrUpdateMenu>(HttpMethod.Post, "/v1/menu", menu);
        }

    }
}
