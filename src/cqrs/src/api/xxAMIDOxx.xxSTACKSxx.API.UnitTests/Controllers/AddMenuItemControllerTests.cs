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

public class AddMenuItemControllerTests
{
    [Fact]
    public void AddMenuItemController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuItemController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new AddMenuItemController(null);

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
        Action action = () => new AddMenuItemController(Substitute.For<ICommandHandler<CreateMenuItem, Guid>>());

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
        typeof(AddMenuItemController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuItemController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuItemController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void CreateMenuItem_Should_BeDecoratedWith_HttpPostAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuItemController)
            .Methods()
            .First(x => x.Name == "AddMenuItem")
            .Should()
            .BeDecoratedWith<HttpPostAttribute>(attribute => attribute.Template == "/v1/menu/{id}/category/{categoryId}/items/");
    }

    [Fact]
    public void CreateMenuItem_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuItemController)
            .Methods()
            .First(x => x.Name == "AddMenuItem")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task CreateMenuItem_Should_Return_StatusCreated()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<ICommandHandler<CreateMenuItem, Guid>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<CreateMenuItem>()).Returns(Task.FromResult(Guid.Empty));

        var body = new CreateItemRequest
        {
            Name = "testName",
            Description = "testDescription"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new AddMenuItemController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.AddMenuItem(Guid.Empty, Guid.Empty, body);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<CreateMenuItem>());

        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode
            .Should()
            .Be(201);
    }
}
