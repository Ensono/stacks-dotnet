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
        /// Adds bearer token to the request based on role name.
        /// When a feature has same behaviour for multiple roles
        /// We could use the same theory and test multiple roles
        /// </summary>
        /// <param name="role">Name of role being authenticated</param>
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


        /// <summary>
        /// Adds an Admin bearer token to the request
        /// </summary>
        public void AsAdmin()
        {
            base.httpClient.AsAdmin();
        }

        /// <summary>
        /// Adds an Employee bearer token to the request
        /// </summary>
        public void AsEmployee()
        {
            base.httpClient.AsEmployee();
        }

        /// <summary>
        /// Adds a Customer bearer token to the request
        /// </summary>
        public void AsCustomer()
        {
            base.httpClient.AsCustomer();
        }

        /// <summary>
        /// Removes any bearer token from the request to simulate unauthenticated user
        /// </summary>
        public void AsUnauthenticatedUser()
        {
            base.httpClient.AsUnauthenticatedUser();
        }

        /// <summary>
        /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
        /// </summary>
        /// <param name="menu">Menu being created</param>
        public async Task<HttpResponseMessage> CreateMenu(CreateOrUpdateMenu menu)
        {
            return await SendAsync<CreateOrUpdateMenu>(HttpMethod.Post, "/v1/menu", menu);
        }

    }
}
