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

public class AddMenuCategoryControllerTests
{
    [Fact]
    public void AddMenuCategoryController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuCategoryController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new AddMenuCategoryController(null);

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
        Action action = () => new AddMenuCategoryController(Substitute.For<ICommandHandler<CreateCategory, Guid>>());

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
        typeof(DeleteCategoryController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteCategoryController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteCategoryController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void AddMenuCategory_Should_BeDecoratedWith_HttpPostAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuCategoryController)
            .Methods()
            .First(x => x.Name == "AddMenuCategory")
            .Should()
            .BeDecoratedWith<HttpPostAttribute>(attribute => attribute.Template == "/v1/menu/{id}/category/");
    }

    [Fact]
    public void AddMenuCategory_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuCategoryController)
            .Methods()
            .First(x => x.Name == "AddMenuCategory")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public void AddMenuCategory_Should_BeDecoratedWith_ProducesResponseTypeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(AddMenuCategoryController)
            .Methods()
            .First(x => x.Name == "AddMenuCategory")
            .Should()
            .BeDecoratedWith<ProducesResponseTypeAttribute>();
    }

    [Fact]
    public async Task AddMenuCategory_Should_Return_StatusCodeCreated()
    {
        // Arrange
        var body = new CreateCategoryRequest
        {
            Name = "testName",
            Description = "testDescription"
        };
        var fakeCommandHandler = Substitute.For<ICommandHandler<CreateCategory, Guid>>();
        var categoryId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<CreateCategory>()).Returns(Task.FromResult(categoryId));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new AddMenuCategoryController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.AddMenuCategory(Guid.Empty, body);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<CreateCategory>());

        result
            .Should()
            .BeOfType<ObjectResult>()
            .Which
            .StatusCode?
            .Should()
            .Be(201);
    }
}
