using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api
{
    public class CrudService
    {
        //ToDo: Update this to use HttpRequestMessage and then use SendAsync
        //This will allow headers to be added
        //This might reduce this class to just one method that sends a request
        //Creating the requests will be the majority of the work. 
        //https://stackoverflow.com/questions/12022965/adding-http-headers-to-httpclient

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
            var byteContent = CreateJsonRequest(jsonBody);

            return await httpClient.PostAsync(path, byteContent);
        }

        public async Task<HttpResponseMessage> PutJson(string path, string jsonBody)
        {
            var byteContent = CreateJsonRequest(jsonBody);

            return await httpClient.PutAsync(path, byteContent);
        }

        private ByteArrayContent CreateJsonRequest(string jsonBody)
        {
            var buffer = Encoding.UTF8.GetBytes(jsonBody);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}
