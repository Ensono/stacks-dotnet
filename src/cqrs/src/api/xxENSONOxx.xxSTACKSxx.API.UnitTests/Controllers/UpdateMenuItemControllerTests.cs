﻿using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Controllers;
using xxENSONOxx.xxSTACKSxx.API.Controllers.Item;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Controllers;

public class UpdateMenuItemControllerTests
{
    [Fact]
    public void UpdateMenuItemController_Should_BeDerivedFrom_ApiControllerBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuItemController)
            .Should()
            .BeDerivedFrom<ApiControllerBase>();
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ICommandHandlerIsNull()
    {
        // Arrange
        // Act
        Action action = () => new UpdateMenuItemController(null);

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
        Action action = () => new UpdateMenuItemController(Substitute.For<ICommandHandler<UpdateMenuItem, bool>>());

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
        typeof(UpdateMenuItemController)
            .Should()
            .BeDecoratedWith<ConsumesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ProducesAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuItemController)
            .Should()
            .BeDecoratedWith<ProducesAttribute>();
    }

    [Fact]
    public void Constructor_Should_BeDecoratedWith_ApiExplorerSettingsAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuItemController)
            .Should()
            .BeDecoratedWith<ApiExplorerSettingsAttribute>();
    }

    [Fact]
    public void UpdateMenuItem_Should_BeDecoratedWith_HttpPutAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuItemController)
            .Methods()
            .First(x => x.Name == "UpdateMenuItem")
            .Should()
            .BeDecoratedWith<HttpPutAttribute>(attribute => attribute.Template == "/v1/menu/{id}/category/{categoryId}/items/{itemId}");
    }

    [Fact]
    public void UpdateMenuItem_Should_BeDecoratedWith_AuthorizeAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateMenuItemController)
            .Methods()
            .First(x => x.Name == "UpdateMenuItem")
            .Should()
            .BeDecoratedWith<AuthorizeAttribute>();
    }
}
