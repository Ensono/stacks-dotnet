﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.Models;

public class UpdateCategoryRequestTests
{
    [Fact]
    public void Name_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(UpdateCategoryRequest)
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
        typeof(UpdateCategoryRequest)
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
        typeof(UpdateCategoryRequest)
            .Properties()
            .First(x => x.Name == "Description")
            .Should()
            .Return<string>();
    }

}
