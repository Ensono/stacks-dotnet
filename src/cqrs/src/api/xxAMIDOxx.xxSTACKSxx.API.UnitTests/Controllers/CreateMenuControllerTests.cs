using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Controllers;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class CreateMenuControllerTests
{
    [Fact]
    public void CreateMenuController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateMenuController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new CreateMenuController(null);

        // Assert
        action
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'commandHandler')");
    }

    [Fact]
    public void Constructor_Should_Not_Throw_When_ICommandHandlerIsPresent()
    {
        // Arrange
        // Act
        Action action = () => new CreateMenuController(Substitute.For<ICommandHandler<CreateMenu, Guid>>());

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
        typeof(CreateMenuController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateMenuController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateMenuController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void CreateMenu_Should_BeDecoratedWith_HttpPostAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateMenuController)
            .Methods()
            .First(x => x.Name == "CreateMenu")
            .Should()
            .BeDecoratedWith<HttpPostAttribute>(attribute => attribute.Template == "/v1/menu/");
    }

    [Fact]
    public void CreateMenu_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateMenuController)
            .Methods()
            .First(x => x.Name == "CreateMenu")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task CreateMenu_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<ICommandHandler<CreateMenu, Guid>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<CreateMenu>()).Returns(Task.FromResult(Guid.Empty));

        var body = new CreateMenuRequest
        {
            Name = "testName",
            Description = "testDescription"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new CreateMenuController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.CreateMenu(body);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<CreateMenu>());

        result
            .Should()
            .BeOfType<CreatedAtActionResult>()
            .Which
            .StatusCode
            .Should()
            .Be(201);
    }
}
