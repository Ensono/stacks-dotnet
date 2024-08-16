using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Swagger.Filters
{
    public class BasePathFilter(string basePath) : IDocumentFilter
    {
        public string BasePath { get; } = basePath;

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(BasePath))
            {
                return;
            }

            swaggerDoc.Servers = new List<OpenApiServer>
            {
                new OpenApiServer {Url = BasePath}
            };
        }
    }
}
