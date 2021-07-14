using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders.Http
{
    public class HttpRequestFactory
    {
        public static async Task<HttpResponseMessage> Get(string baseUrl, 
            string path, 
            string authToken = null, 
            Dictionary<string, string> headers = null, 
            Dictionary<string,string> parameters = null)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(baseUrl, path)
                                .AddBearerToken(authToken)
                                .AddCustomHeaders(headers)
                                .AddParameters(parameters)
                                .SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(
            string baseUrl, 
            string path, 
            object body, 
            string authToken = null, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(baseUrl, path)
                                .AddBearerToken(authToken)
                                .AddContent(CreateHttpContent(body))
                                .AddCustomHeaders(headers)
                                .AddParameters(parameters)
                                .SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(
            string baseUrl, 
            string path, 
            object body, 
            string authToken = null, 
            Dictionary<string,string> headers = null, 
            Dictionary<string,string> parameters = null)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(baseUrl, path)
                                .AddBearerToken(authToken)
                                .AddContent(CreateHttpContent(body))
                                .AddCustomHeaders(headers)
                                .AddParameters(parameters)
                                .SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(
            string baseUrl, 
            string path, 
            string authToken = null, 
            Dictionary<string,string> headers = null)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(baseUrl, path)
                                .AddBearerToken(authToken)
                                .AddCustomHeaders(headers)
                                .SendAsync();
        }

        //Creates HttpContent from the object provided
        private static HttpContent CreateHttpContent<TContent>(TContent content)
        {
            //If the content is empty then create empty HttpContent
            if (EqualityComparer<TContent>.Default.Equals(content, default(TContent)))
            {
                return new ByteArrayContent(new byte[0]);
            }
            //if the content is not empty, then create HttpContent with the Accept header set to 'application/json'
            else
            {
                var json = JsonConvert.SerializeObject(content);
                var result = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return result;
            }
        }
    }
}
