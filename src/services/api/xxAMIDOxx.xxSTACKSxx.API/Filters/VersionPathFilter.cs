using System.Linq;
using System.Text.RegularExpressions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxAMIDOxx.xxSTACKSxx.API.Filters
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

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!this.BasePath.Contains(swaggerDoc.Info.Version))
                return;

            var pathsToModify = swaggerDoc.Paths.ToList();
            foreach (var path in pathsToModify)
            {
                if (!path.Key.StartsWith(this.BasePath))
                {
                    swaggerDoc.Paths.Remove(path.Key);
                }
            }
        }
    }
}