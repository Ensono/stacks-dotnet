using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amido.Stacks.E2e.Tests.Api
{
    public class CrudService
    {
        private static HttpClient _httpClient = new HttpClient();

        public CrudService(string baseUri)
        {
            _httpClient.BaseAddress = new Uri(baseUri);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
        }

        public async Task<HttpResponseMessage> Get(string path)
        {
            return await _httpClient.GetAsync(path);
        }
    }
}
