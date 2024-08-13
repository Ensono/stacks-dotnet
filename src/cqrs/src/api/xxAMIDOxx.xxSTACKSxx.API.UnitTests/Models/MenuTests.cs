using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Models;

public class MenuTests
{
    [Fact]
    public void Id_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Id")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Id_Should_ReturnGuid()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Id")
            .Should()
            .Return<Guid?>();
    }

    [Fact]
    public void Name_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Name")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Name_Should_ReturnString()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Name")
            .Should()
            .Return<string>();
    }

    [Fact]
    public void Description_Should_ReturnString()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Description")
            .Should()
            .Return<string>();
    }

    [Fact]
    public void Categories_Should_ReturnList()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Categories")
            .Should()
            .Return<List<Category>>();
    }

    [Fact]
    public void Enabled_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Enabled")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Enabled_Should_ReturnBool()
    {
        // Arrange
        // Act
        // Assert
        typeof(Menu)
            .Properties()
            .First(x => x.Name == "Enabled")
            .Should()
            .Return<bool?>();
    }
}
