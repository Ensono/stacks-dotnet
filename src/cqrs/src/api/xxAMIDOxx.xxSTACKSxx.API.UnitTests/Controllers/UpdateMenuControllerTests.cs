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

public class UpdateMenuControllerTests
{
    [Fact]
    public void UpdateMenuController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new UpdateMenuController(null);

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
        Action action = () => new UpdateMenuController(Substitute.For<ICommandHandler<UpdateMenu, bool>>());

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
        typeof(UpdateMenuController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void UpdateMenu_Should_BeDecoratedWith_HttpPutAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuController)
            .Methods()
            .First(x => x.Name == "UpdateMenu")
            .Should()
            .BeDecoratedWith<HttpPutAttribute>(attribute => attribute.Template == "/v1/menu/{id}");
    }

    [Fact]
    public void UpdateMenu_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuController)
            .Methods()
            .First(x => x.Name == "UpdateMenu")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task UpdateMenu_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<ICommandHandler<UpdateMenu, bool>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<UpdateMenu>()).Returns(Task.FromResult(true));

        var body = new UpdateMenuRequest
        {
            Name = "testName",
            Description = "testDescription"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new UpdateMenuController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.UpdateMenu(Guid.Empty, body);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<UpdateMenu>());

        result
            .Should()
            .BeOfType<StatusCodeResult>()
            .Which
            .StatusCode
            .Should()
            .Be(204);
    }
}
