using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Authentication;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.JwtBearerAuthentication;

public class JwtBearerAuthenticationOperationFilterTests
{
    [Fact]
    public void Apply_Adds401Response_WhenAuthorizeAttributeIsPresent()
    {
        var operation = new OpenApiOperation();
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(new JsonSerializerOptions())),
            new SchemaRepository(),
            typeof(TestController).GetMethod(nameof(TestController.AuthorizedMethod))
        );

        var filter = new JwtBearerAuthenticationOperationFilter();
        filter.Apply(operation, context);

        Assert.Contains("401", operation.Responses.Keys);
        Assert.Equal("Unauthorized. Access token is missing or invalid.", operation.Responses["401"].Description);
    }

    [Fact]
    public void Apply_Adds403Response_WhenAuthorizeAttributeIsPresent()
    {
        var operation = new OpenApiOperation();
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(new JsonSerializerOptions())),
            new SchemaRepository(),
            typeof(TestController).GetMethod(nameof(TestController.AuthorizedMethod))
        );

        var filter = new JwtBearerAuthenticationOperationFilter();
        filter.Apply(operation, context);

        Assert.Contains("403", operation.Responses.Keys);
        Assert.Equal("Forbidden. The user does not have permission to execute this operation.",
            operation.Responses["403"].Description);
    }

    [Fact]
    public void Apply_DoesNotAddResponses_WhenAuthorizeAttributeIsNotPresent()
    {
        var operation = new OpenApiOperation();
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(new JsonSerializerOptions())),
            new SchemaRepository(),
            typeof(TestController).GetMethod(nameof(TestController.NonAuthorizedMethod))
        );

        var filter = new JwtBearerAuthenticationOperationFilter();
        filter.Apply(operation, context);

        Assert.DoesNotContain("401", operation.Responses.Keys);
        Assert.DoesNotContain("403", operation.Responses.Keys);
    }

    [Fact]
    public void Apply_AddsSecurityRequirements_WhenAuthorizeAttributeIsPresent()
    {
        var operation = new OpenApiOperation();
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(new JsonSerializerOptions())),
            new SchemaRepository(),
            typeof(TestController).GetMethod(nameof(TestController.AuthorizedMethod))
        );

        var filter = new JwtBearerAuthenticationOperationFilter();
        filter.Apply(operation, context);

        Assert.NotNull(operation.Security);
        Assert.NotEmpty(operation.Security);
        Assert.Contains(operation.Security,
            s => s.ContainsKey(new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = OpenApiSecurityDefinitions.OAuth2, Type = ReferenceType.SecurityScheme
                }
            }));
        Assert.Contains(operation.Security,
            s => s.ContainsKey(new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = OpenApiSecurityDefinitions.Bearer, Type = ReferenceType.SecurityScheme
                }
            }));
    }
}

[ExcludeFromCodeCoverage]
internal class TestController
{
    [Authorize]
    public async Task<IActionResult> AuthorizedMethod()
    {
        return await Task.FromResult(new OkResult());
    }
    
    public async Task<IActionResult> NonAuthorizedMethod()
    {
        return await Task.FromResult(new OkResult());
    }
}
