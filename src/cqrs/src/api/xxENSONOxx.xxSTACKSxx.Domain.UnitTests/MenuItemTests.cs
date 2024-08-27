using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Domain.Entities;

namespace xxENSONOxx.xxSTACKSxx.Domain.UnitTests;

[Trait("TestType", "UnitTests")]
public class MenuItemTests
{
    [Theory, AutoData]
    public void Constructor(
        Guid categoryId,
        string name,
        string description,
        double price,
        bool available)
    {
        // Arrange
        // Act
        var menuItem = new MenuItem(categoryId, name, description, price, available);

        // Assert
        menuItem.Id.Should().Be(categoryId);
        menuItem.Name.Should().Be(name);
        menuItem.Description.Should().Be(description);
        menuItem.Price.Should().Be(price);
        menuItem.Available.Should().Be(available);
    }
}
