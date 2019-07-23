using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class HttpRequestFactory
    {
        public static async Task<HttpResponseMessage> Get(string requestUri)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .SendAsync();
        }


        public static async Task<HttpResponseMessage> Post(string requestUri, object body)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(CreateHttpContent(body))
                                .SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, object body)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(CreateHttpContent(body))
                                .SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri)
        {
            return await new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .SendAsync();
        }

        private static HttpContent CreateHttpContent<TContent>(TContent content)
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
