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
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class GetMenuByIdControllerTests
{
    [Fact]
    public void GetMenuByIdController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new GetMenuByIdController(null);

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
        Action action = () => new GetMenuByIdController(Substitute.For<IQueryHandler<GetMenuById, Menu>>());

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
        typeof(GetMenuByIdController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void UpdateCategory_Should_BeDecoratedWith_HttpPutAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdController)
            .Methods()
            .First(x => x.Name == "GetMenu")
            .Should()
            .BeDecoratedWith<HttpGetAttribute>(attribute => attribute.Template == "/v1/menu/{id}");
    }

    [Fact]
    public void UpdateCategory_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(GetMenuByIdController)
            .Methods()
            .First(x => x.Name == "GetMenu")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task UpdateCategory_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<IQueryHandler<GetMenuById, Menu>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.ExecuteAsync(Arg.Any<GetMenuById>()).Returns(Task.FromResult(new Menu()));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new GetMenuByIdController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.GetMenu(Guid.Empty);

        // Assert
        await fakeCommandHandler.Received().ExecuteAsync(Arg.Any<GetMenuById>());

        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .Value
            .Should()
            .BeOfType<xxAMIDOxx.xxSTACKSxx.API.Models.Responses.Menu>();
    }

    [Fact]
    public async Task UpdateCategory_Should_Return_StatusCodeNotFound()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<IQueryHandler<GetMenuById, Menu>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.ExecuteAsync(Arg.Any<GetMenuById>())!.Returns(default(Menu));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new GetMenuByIdController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.GetMenu(Guid.Empty);

        // Assert
        await fakeCommandHandler.Received().ExecuteAsync(Arg.Any<GetMenuById>());

        result
            .Should()
            .BeOfType<NotFoundResult>();
    }
}
