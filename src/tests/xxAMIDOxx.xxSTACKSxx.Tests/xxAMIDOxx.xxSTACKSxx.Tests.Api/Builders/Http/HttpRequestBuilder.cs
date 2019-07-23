using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders.Http
{
    public class HttpRequestBuilder
    {
        private HttpMethod method;
        private string path;
        private string baseUrl;
        private HttpContent content;
        private string bearerToken;
        private string acceptHeader;

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string baseUrl, string requestUri)
        {
            this.baseUrl = baseUrl;
            this.path = requestUri;

            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            //Create the request message based on the request in the builder
            var request = new HttpRequestMessage
            {
                Method = this.method,
                RequestUri = new Uri($"{this.baseUrl}{this.path}")
            };

            //Add content if present in the request
            if (this.content != null)
            {
                request.Content = this.content;
            }

            //Add bearer token if present in the request
            if (!string.IsNullOrEmpty(this.bearerToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer " + this.bearerToken);
            }

            //Clear then add the Accept header if if exists in the request
            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.acceptHeader))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.acceptHeader));
            }

            //Creates or Gets an existing HttpClient for the BaseUrl being used
            var httpClient = HttpClientFactory.GetHttpClientInstance(baseUrl);

            return await httpClient.SendAsync(request);
        }
    }


    //This static factory ensures that we are using one HttpClient per BaseUrl used in the solution.
    //This prevents a large number sockets being left open after the tests are run
    public static class HttpClientFactory
    {
        private static ConcurrentDictionary<string, HttpClient> httpClientList = new ConcurrentDictionary<string, HttpClient>();

        public static HttpClient GetHttpClientInstance(string baseUrl)
        {
            if (!httpClientList.ContainsKey(baseUrl))
                httpClientList.TryAdd(baseUrl, new HttpClient());

            return httpClientList[baseUrl];
        }
    }
}
