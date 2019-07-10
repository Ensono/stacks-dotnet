using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures
{
    /// <summary>
    /// This class will generate the token to the user specified
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

        private static void AddTokenToClient(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
