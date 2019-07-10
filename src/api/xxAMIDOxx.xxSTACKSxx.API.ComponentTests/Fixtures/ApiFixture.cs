using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.API.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures
{
    public class ApiFixture
    {
        HttpClient _httpClient;
        public HttpResponseMessage LastResponse { get; private set; }

        public ApiFixture()
        {
            //_documentRepository = Substitute.For<IDocumentRepository<FavouritesEntity, Guid>>();

            var webAppFactory = new WebAppFactory();
            //(collection =>
            //{
            //    collection.AddSingleton(_documentRepository);
            //});

            _httpClient = webAppFactory.CreateClient();
        }

        public void AsAdmin()
        {
            _httpClient.AsAdmin();
        }

        public void AsEmployee()
        {
            _httpClient.AsEmployee();
        }

        public void AsCustomer()
        {
            _httpClient.AsCustomer();
        }

        public void AsUserWithExpiredToken()
        {
            _httpClient.AsUserWithExpiredToken();
        }

        public async Task CreateMenu(CreateOrUpdateMenu menu)
        {
            LastResponse = await _httpClient.PostAsync("/v1/menu", CreateHttpContent<CreateOrUpdateMenu>(menu));
        }

        //TODO: Move this to base ApiFixture, Rename this to ApiClientFixture
        private HttpContent CreateHttpContent<TContent>(TContent content)
        {
            if (EqualityComparer<TContent>.Default.Equals(content, default(TContent)))
            {
                //Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} as empty");
                return new ByteArrayContent(new byte[0]);
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                //Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} into {json}");
                var result = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return result;
            }
        }
    }
}
