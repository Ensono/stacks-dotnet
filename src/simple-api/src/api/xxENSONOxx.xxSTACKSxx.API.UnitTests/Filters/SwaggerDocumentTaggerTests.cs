using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Filters;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Filters;

public class SwaggerDocumentTaggerTests
{
    [Fact]
    public void Apply_SetsTags_WhenVersionMatches()
    {
        var tags = new[] { new OpenApiTag { Name = "Tag1" }, new OpenApiTag { Name = "Tag2" } };
        var versions = new[] { "v1" };
        var filter = new SwaggerDocumentTagger(tags, versions);
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Tags = new List<OpenApiTag>()
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Equal(tags, swaggerDoc.Tags);
    }

    [Fact]
    public void Apply_DoesNotSetTags_WhenVersionDoesNotMatch()
    {
        var tags = new[] { new OpenApiTag { Name = "Tag1" }, new OpenApiTag { Name = "Tag2" } };
        var versions = new[] { "v2" };
        var filter = new SwaggerDocumentTagger(tags, versions);
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Tags = new List<OpenApiTag>()
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Empty(swaggerDoc.Tags);
    }

    [Fact]
    public void Apply_SetsTags_WhenNoVersionsSpecified()
    {
        var tags = new[] { new OpenApiTag { Name = "Tag1" }, new OpenApiTag { Name = "Tag2" } };
        var filter = new SwaggerDocumentTagger(tags, null);
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Tags = new List<OpenApiTag>()
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Equal(tags, swaggerDoc.Tags);
    }
}
