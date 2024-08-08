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

public class DeleteCategoryControllerTests
{
    [Fact]
    public void DeleteCategoryController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteCategoryController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new DeleteCategoryController(null);

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
        Action action = () => new DeleteCategoryController(Substitute.For<ICommandHandler<DeleteCategory, bool>>());

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
    public void DeleteCategory_Should_BeDecoratedWith_HttpDeleteAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteCategoryController)
            .Methods()
            .First(x => x.Name == "DeleteCategory")
            .Should()
            .BeDecoratedWith<HttpDeleteAttribute>(attribute => attribute.Template == "/v1/menu/{id}/category/{categoryId}");
    }

    [Fact]
    public void DeleteCategory_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(DeleteCategoryController)
            .Methods()
            .First(x => x.Name == "DeleteCategory")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }

    [Fact]
    public async Task DeleteCategory_Should_Return_StatusCodeNoContent()
    {
        // Arrange
        var fakeCommandHandler = Substitute.For<ICommandHandler<DeleteCategory, bool>>();
        var correlationId = Guid.NewGuid();
        fakeCommandHandler.HandleAsync(Arg.Any<DeleteCategory>()).Returns(Task.FromResult(true));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["x-correlation-id"] = correlationId.ToString();

        var sut = new DeleteCategoryController(fakeCommandHandler)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            }
        };

        // Act
        var result = await sut.DeleteCategory(Guid.Empty, Guid.Empty);

        // Assert
        await fakeCommandHandler.Received().HandleAsync(Arg.Any<DeleteCategory>());

        result
            .Should()
            .BeOfType<StatusCodeResult>()
            .Which
            .StatusCode
            .Should()
            .Be(204);
    }
}
