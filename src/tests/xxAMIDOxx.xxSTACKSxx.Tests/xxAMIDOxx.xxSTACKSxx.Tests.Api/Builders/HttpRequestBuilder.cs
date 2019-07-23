using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class HttpRequestBuilder
    {
        private HttpMethod method;
        private string requestUri;
        private HttpContent content;
        private string bearerToken;
        private string acceptHeader;
        private TimeSpan timeout;
        private ConfigModel config;


        public HttpRequestBuilder()
        {
            config = ConfigAccessor.GetApplicationConfiguration();
        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = config.BaseUrl + requestUri;
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

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = this.method,
                RequestUri = new Uri(this.requestUri)
            };

            if(this.content != null)
            {
                request.Content = this.content;
            }

            if(!string.IsNullOrEmpty(this.bearerToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer " + this.bearerToken);
            }

            request.Headers.Accept.Clear();
            if(!string.IsNullOrEmpty(this.acceptHeader))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.acceptHeader));
            }

            return await new HttpClient().SendAsync(request);
        }
    }
}
