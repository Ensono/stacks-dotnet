using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Amido.Stacks.API.Swagger.Filters
{
    /// <summary>
    /// Filter operation based on path versioning
    /// </summary>
    public class VersionPathFilter : IDocumentFilter
    {
        public VersionPathFilter(string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!BasePath.Contains(swaggerDoc.Info.Version))
                return;

            var pathsToModify = swaggerDoc.Paths.ToList();
            foreach (var path in pathsToModify)
            {
                if (!path.Key.StartsWith(BasePath))
                {
                    swaggerDoc.Paths.Remove(path.Key);
                }
            }
        }
    }
}
