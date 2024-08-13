using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Models;

public class CategoryTests
{
    [Fact]
    public void Id_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(Category)
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
        typeof(Category)
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
        typeof(Category)
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
        typeof(Category)
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
        typeof(Category)
            .Properties()
            .First(x => x.Name == "Description")
            .Should()
            .Return<string>();
    }

    [Fact]
    public void Items_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(Category)
            .Properties()
            .First(x => x.Name == "Items")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Items_Should_ReturnList()
    {
        // Arrange
        // Act
        // Assert
        typeof(Category)
            .Properties()
            .First(x => x.Name == "Items")
            .Should()
            .Return<List<Item>>();
    }
}
