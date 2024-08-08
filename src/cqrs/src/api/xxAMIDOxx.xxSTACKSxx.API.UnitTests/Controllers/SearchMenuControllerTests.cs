using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class SearchMenuControllerTests
{
    [Fact]
    public void SearchMenuController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new SearchMenuController(null);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'queryHandler')");
    }

    [Fact]
    public void Constructor_Should_Not_Throw_When_ICommandHandlerIsPresent()
    {
        // Arrange
        // Act
        Action action = () => new SearchMenuController(Substitute.For<IQueryHandler<SearchMenu, SearchMenuResult>>());

        // Assert
        action
            .Should()
            .NotThrow();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ConsumesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void UpdateCategory_Should_BeDecoratedWith_HttpPutAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Methods()
            .First(x => x.Name == "SearchMenu")
            .Should()
            .BeDecoratedWith<HttpGetAttribute>(attribute => attribute.Template == "/v1/menu/");
    }

    [Fact]
    public void UpdateCategory_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuController)
            .Methods()
            .First(x => x.Name == "SearchMenu")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task UpdateCategory_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<IQueryHandler<SearchMenu, SearchMenuResult>>();
        var correlationId = Guid.NewGuid();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new SearchMenuController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.SearchMenu(string.Empty, Guid.Empty);

        // Assert
        await fakeCommandHandler.Received().ExecuteAsync(Arg.Any<SearchMenu>());

        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .Value
            .Should()
            .BeOfType<SearchMenuResponse>();
    }
}
