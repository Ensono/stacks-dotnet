using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxENSONOxx.xxSTACKSxx.API.Filters;

/// <summary>
/// This filter will modify the document with default tags
/// which are shown on the top in order
/// other tags not defined in the document will be shown at the bottom
/// </summary>
public class SwaggerDocumentTagger : IDocumentFilter
{
    /// <summary>
    /// Apply default tags to a swagger doc
    /// </summary>
    /// <param name="tags">tags to be applied to the api doc</param>
    /// <param name="versions">when specified, will apply only to documets of specific versions</param>
    public SwaggerDocumentTagger(OpenApiTag[] tags, string[] versions)
    {
        Tags = tags;
        Versions = versions;
    }

    public OpenApiTag[] Tags { get; }
    public string[] Versions { get; }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //if a version is specified, only applies the rule to the version
        if (Versions != null && Versions.Length > 0 && !Versions.Any(v => v == swaggerDoc.Info.Version))
            return;

        swaggerDoc.Tags = Tags;
    }
}
