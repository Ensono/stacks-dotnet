using System.Net.Http;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures
{
    /// <summary>
    /// This class will add the auth token to the client requests
    /// By default static tokens with long expiration should be used
    /// For dynamic generation, refactor it to retrieve the tokens from 
    /// token provider
    /// </summary>
    public static class AuthTokenFixture
    {
        private const string AdminToken = "";
        private const string EmployeeToken = "";
        private const string CustomerToken = "";
        private const string ExpiredToken = "";

        public static void AsAdmin(this HttpClient client)
        {
            AddTokenToClient(client, AdminToken);
        }

        public static void AsEmployee(this HttpClient client)
        {
            AddTokenToClient(client, EmployeeToken);
        }

        public static void AsCustomer(this HttpClient client)
        {
            AddTokenToClient(client, CustomerToken);
        }

        public static void AsUserWithExpiredToken(this HttpClient client)
        {
            AddTokenToClient(client, ExpiredToken);
        }
        public static void AsUnauthenticatedUser(this HttpClient client)
        {
            RemoveTokenFromClient(client);
        }

        private static void AddTokenToClient(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        private static void RemoveTokenFromClient(HttpClient client)
        {
            if (client.DefaultRequestHeaders.Contains("Authorization"))
                client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
