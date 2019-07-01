using System.Linq;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxAMIDOxx.xxSTACKSxx.API.Filters
{
    /// <summary>
    /// Adds a base path to the spec 
    /// Required when app running behind a proxy\gateway
    /// i.e: domain.com/api/menu/v1/health -> domain.com/v1/health 
    /// The gateway addzremove the "/api/menu" so the BasePath should be "/api/menu"
    /// </summary>
    public class BasePathFilter : IDocumentFilter
    {
        public BasePathFilter(string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(BasePath))
                return;

            swaggerDoc.BasePath = BasePath;
        }
    }
}