using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests
{
    //TODO: Decide if we move this to a core package to be reused by multiple test projects

    /// <summary>
    /// ApiFixture will handle the HttpClient creation for the test scenarios.
    /// Each fixture talking to the API should inherit from this class
    /// </summary>
    /// <typeparam name="TStartup">The Startup file from the API project</typeparam>
    public abstract class ApiFixture<TStartup> where TStartup : class
    {
        protected HttpClient httpClient;

        public HttpResponseMessage LastResponse { get; protected set; }

        public ApiFixture()
        {
            var webAppFactory = new WebAppFactory<TStartup>(RegisterDependencies, ConfigureWebHost);
            httpClient = webAppFactory.CreateClient();
        }

        /// <summary>
        /// Send the request and set the LastReponse
        /// </summary>
        /// <param name="method">Http method used in the request</param>
        /// <param name="url">relative url for API resource</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url)
        {
            return await SendAsync(method, url, (string)null);
        }

        /// <summary>
        /// Send the request and set the LastReponse
        /// </summary>
        /// <typeparam name="TBody">Type of bdy being sent</typeparam>
        /// <param name="method">Http method used in the request</param>
        /// <param name="url">relative url for API resource</param>
        /// <param name="body">body to be submitted to the API</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendAsync<TBody>(HttpMethod method, string url, TBody body)
        {
            lastResponseObject = null;

            HttpRequestMessage msg = new HttpRequestMessage(method, url);

            if (body != null)
                msg.Content = CreateHttpContent<TBody>(body);

            LastResponse = await httpClient.SendAsync(msg);

            return LastResponse;
        }


        /// <summary>
        /// Configure WebHost(if needed) before the API Startup is added to the pipeline
        /// </summary>
        protected virtual void ConfigureWebHost(IWebHostBuilder builder) { }

        /// <summary>
        /// Register the dependent services needed in the API
        /// </summary>
        protected abstract void RegisterDependencies(IServiceCollection collection);

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

        object lastResponseObject;
        internal async Task<TBody> GetResponseObject<TBody>()
        {
            if (lastResponseObject == null)
                lastResponseObject = await LastResponse.Content.ReadAsAsync<TBody>();

            return (TBody)lastResponseObject;
        }
    }
}
