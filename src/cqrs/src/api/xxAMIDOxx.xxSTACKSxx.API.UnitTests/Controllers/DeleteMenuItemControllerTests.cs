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
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class DeleteMenuItemControllerTests
{
    [Fact]
    public void DeleteMenuItemController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteMenuItemController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new DeleteMenuItemController(null);

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
        Action action = () => new DeleteMenuItemController(Substitute.For<ICommandHandler<DeleteMenuItem, bool>>());

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
        typeof(DeleteMenuItemController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteMenuItemController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteMenuItemController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void DeleteMenuItem_Should_BeDecoratedWith_HttpPutAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteMenuItemController)
            .Methods()
            .First(x => x.Name == "DeleteMenuItem")
            .Should()
            .BeDecoratedWith<HttpDeleteAttribute>(attribute => attribute.Template == "/v1/menu/{id}/category/{categoryId}/items/{itemId}");
    }

    [Fact]
    public void DeleteMenuItem_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteMenuItemController)
            .Methods()
            .First(x => x.Name == "DeleteMenuItem")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task DeleteMenuItem_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<ICommandHandler<DeleteMenuItem, bool>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<DeleteMenuItem>()).Returns(Task.FromResult(true));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new DeleteMenuItemController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.DeleteMenuItem(Guid.Empty, Guid.Empty, Guid.Empty);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<DeleteMenuItem>());

        result
            .Should()
            .BeOfType<StatusCodeResult>()
            .Which
            .StatusCode
            .Should()
            .Be(204);
    }
}
