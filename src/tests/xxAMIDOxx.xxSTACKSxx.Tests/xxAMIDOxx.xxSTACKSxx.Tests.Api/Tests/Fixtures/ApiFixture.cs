using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Tests.Fixtures
{
    /// <summary>
    /// ApiFixture will handle the HttpClient creation for the test scenarios.
    /// Each fixture talking to the API should inherit from this class
    /// </summary>
    public abstract class ApiFixture
    {
        private static HttpClient httpClient;

        public ApiFixture()
        {
            Debug.WriteLine("API Fixture constructor");
            //Create the factory here
            if(httpClient == null)
            {
                httpClient = BuildHttpClient();
            }
        }

        public abstract HttpClient BuildHttpClient();

        /// <summary>
        /// Send the request and set the LastReponse
        /// </summary>
        /// <param name="method">Http method used in the request</param>
        /// <param name="url">relative url for API resource</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url)
        {
            HttpRequestMessage msg = new HttpRequestMessage(method, url);

            return await httpClient.SendAsync(msg);
        }

        /// <summary>
        /// Send the request and set the LastReponse
        /// </summary>
        /// <typeparam name="TBody">Object to be used in the request body</typeparam>
        /// <param name="method">Http method used in the request</param>
        /// <param name="url">relative url for API resource</param>
        /// <param name="body">body to be submitted to the API</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendAsync<TBody>(HttpMethod method, string url, TBody body)
        {
            HttpRequestMessage msg = new HttpRequestMessage(method, url);

            if (body != null)
                msg.Content = CreateHttpContent<TBody>(body);

            return await httpClient.SendAsync(msg);
        }

        protected HttpContent CreateHttpContent<TContent>(TContent content)
        {
            if (EqualityComparer<TContent>.Default.Equals(content, default(TContent)))
            {
                Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} as empty");
                return new ByteArrayContent(new byte[0]);
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} into {json}");
                var result = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return result;
            }
        }
    }
}
