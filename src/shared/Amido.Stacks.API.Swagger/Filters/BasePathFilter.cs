using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Amido.Stacks.API.Swagger.Filters
{
    public class BasePathFilter : IDocumentFilter
    {
        public BasePathFilter(string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; }

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