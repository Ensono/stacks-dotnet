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
        // User ids need to match the sub claim in the corresponding tokens
        private const string AdminUserId = "";
        private const string EmployeeUserId = "";
        private const string CustomerUserId = "";

        // TODO - Set test bearer tokens here
        private const string AdminToken = "";
        private const string EmployeeToken = "";
        private const string CustomerToken = "";

        public static string AsAdmin(this HttpClient client)
        {
            RemoveTokenFromClient(client);
            AddTokenToClient(client, AdminToken);
            return AdminUserId;
        }

        public static string AsEmployee(this HttpClient client)
        {
            RemoveTokenFromClient(client);
            AddTokenToClient(client, EmployeeToken);
            return EmployeeUserId;
        }

        public static string AsCustomer(this HttpClient client)
        {
            RemoveTokenFromClient(client);
            AddTokenToClient(client, CustomerToken);
            return CustomerUserId;
        }

        public static string AsUnauthenticatedUser(this HttpClient client)
        {
            RemoveTokenFromClient(client);
            return null;
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
