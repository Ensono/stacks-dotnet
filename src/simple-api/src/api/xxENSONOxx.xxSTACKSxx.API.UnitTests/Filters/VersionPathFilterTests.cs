using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Filters;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Filters;

public class VersionPathFilterTests
{
    [Fact]
    public void Apply_RemovesPathsNotStartingWithBasePath()
    {
        var filter = new VersionPathFilter("/v1");
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Paths = new OpenApiPaths
            {
                ["/v1/resource"] = new(),
                ["/v2/resource"] = new()
            }
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Single(swaggerDoc.Paths);
        Assert.Contains("/v1/resource", swaggerDoc.Paths.Keys);
    }

    [Fact]
    public void Apply_DoesNotRemovePathsWhenBasePathContainsVersion()
    {
        var filter = new VersionPathFilter("/v1");
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Paths = new OpenApiPaths
            {
                ["/v1/resource"] = new(),
                ["/v1/anotherResource"] = new()
            }
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Equal(2, swaggerDoc.Paths.Count);
    }

    [Fact]
    public void Apply_DoesNothingWhenBasePathDoesNotContainVersion()
    {
        var filter = new VersionPathFilter("/v2");
        var swaggerDoc = new OpenApiDocument
        {
            Info = new OpenApiInfo { Version = "v1" },
            Paths = new OpenApiPaths
            {
                ["/v1/resource"] = new(),
                ["/v2/resource"] = new()
            }
        };
        var context = new DocumentFilterContext(null, null, null);

        filter.Apply(swaggerDoc, context);

        Assert.Equal(2, swaggerDoc.Paths.Count);
    }
}
