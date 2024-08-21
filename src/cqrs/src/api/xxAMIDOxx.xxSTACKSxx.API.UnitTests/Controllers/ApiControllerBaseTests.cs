using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class ApiControllerBaseTests
{
    [Fact]
    public void ApiControllerBase_Should_BeDerivedFrom_ControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiControllerBase)
            .Should()
            .BeDerivedFrom<ControllerBase>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ConsumesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiControllerBase)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiControllerBase)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiControllerBase)
            .Should()
            .BeDecoratedWith<ApiControllerAttribute>();
    }

    [Fact]
    public void GetCorrelationId_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(ApiControllerBase)
            .Methods()
            .First(x => x.Name == "GetCorrelationId")
            .Should()
            .BeDecoratedWith<NonActionAttribute>();
    }

    [Fact]
    public void GetCorrelationId_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var correlationId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new ApiControllerBase
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = sut.GetCorrelationId();

        // Assert
        result
            .Should()
            .Be(correlationId);
    }
}
