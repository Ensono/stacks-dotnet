using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Amido.Stacks.API.Swagger.Filters
{
    /// <summary>
    /// This filter will modify the document with default tags
    /// Default tags are shown on the top in order
    /// other tags not defined in the document will be shown at the bottom
    /// </summary>
    public class SwaggerDocumentTagger : IDocumentFilter
    {
        /// <summary>
        /// Apply default tags to a swagger doc
        /// </summary>
        /// <param name="tags">tags to be applied to the api doc</param>
        /// <param name="versions">when specified, will apply only to documets of specific versions</param>
        public SwaggerDocumentTagger(Tag[] tags, string[] versions)
        {
            Tags = tags;
            Versions = versions;
        }

        public Tag[] Tags { get; }
        public string[] Versions { get; }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //if a version is specified, only applies the rule to the version
            if (Versions != null && Versions.Length > 0 && !Versions.Any(v => v == swaggerDoc.Info.Version))
                return;

            swaggerDoc.Tags = Tags;
        }
    }
}
