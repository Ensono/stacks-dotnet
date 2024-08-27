using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace xxENSONOxx.xxSTACKSxx.Shared.API.Models
{
    public class BadResult : ProblemDetails
    {
        private const int DefaultStatusCode = 400;

        public BadResult()
        {
            Status = DefaultStatusCode;
        }

        public BadResult(int statusCode)
        {
            Status = statusCode;
        }

        public BadResult(int statusCode, string traceId)
        {
            Status = statusCode;
            TraceId = traceId;
        }

        public string TraceId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, settings);
        }

        static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
