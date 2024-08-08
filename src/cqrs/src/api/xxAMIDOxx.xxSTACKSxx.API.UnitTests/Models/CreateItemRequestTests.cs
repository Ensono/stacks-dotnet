using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Models;

public class CreateItemRequestTests
{
    [Fact]
    public void Name_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateItemRequest)
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
        typeof(CreateItemRequest)
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
        typeof(CreateItemRequest)
            .Properties()
            .First(x => x.Name == "Description")
            .Should()
            .Return<string>();
    }

    [Fact]
    public void Price_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateItemRequest)
            .Properties()
            .First(x => x.Name == "Price")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Price_Should_ReturnDouble()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateItemRequest)
            .Properties()
            .First(x => x.Name == "Price")
            .Should()
            .Return<double>();
    }

    [Fact]
    public void Available_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateItemRequest)
            .Properties()
            .First(x => x.Name == "Available")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Available_Should_ReturnBool()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateItemRequest)
            .Properties()
            .First(x => x.Name == "Available")
            .Should()
            .Return<bool>();
    }
}
