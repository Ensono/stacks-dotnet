using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api
{
    public class CrudService
    {
        private HttpClient httpClient;

        public CrudService(string baseUri)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri),
                Timeout = new TimeSpan(0, 0, 30)
            };
        }

        public async Task<HttpResponseMessage> Get(string path)
        {
            return await httpClient.GetAsync(path);
        }

        public async Task<HttpResponseMessage> PostJson(string path, string jsonBody)
        {
            var buffer = Encoding.UTF8.GetBytes(jsonBody);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await httpClient.PostAsync(path, byteContent);
        }
    }
}
