using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class GetMenuByIdV2ControllerTests
{
    [Fact]
    public void GetMenuByIdV2Controller_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ConsumesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiControllerAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Should()
            .BeDecoratedWith<ApiControllerAttribute>();
    }

    [Fact]
    public void GetMenuV2_Should_BeDecoratedWith_HttpGetAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Methods()
            .First(x => x.Name == "GetMenuV2")
            .Should()
            .BeDecoratedWith<HttpGetAttribute>(attribute => attribute.Template == "/v2/menu/{id}");
    }

    [Fact]
    public void GetMenuV2_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Methods()
            .First(x => x.Name == "GetMenuV2")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public void GetMenuV2_Should_BeDecoratedWith_ProducesResponseTypeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdV2Controller)
            .Methods()
            .First(x => x.Name == "GetMenuV2")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>();
    }

    [Fact]
    public void GetMenuV2_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var sut = new GetMenuByIdV2Controller();

        // Act
        var result = sut.GetMenuV2(Guid.Empty);

        // Assert
        result
            .Should()
            .BeOfType<BadRequestResult>();
    }
}
