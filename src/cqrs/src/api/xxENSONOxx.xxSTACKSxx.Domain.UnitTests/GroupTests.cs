using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.ValueObjects;
using xxENSONOxx.xxSTACKSxx.Domain.ValueObjects;

namespace xxENSONOxx.xxSTACKSxx.Domain.UnitTests;

[Trait("TestType", "UnitTests")]
public class GroupTests
{
    [Theory, AutoData]
    public void Constructor(
        Guid categoryId,
        string name)
    {
        // Arrange
        // Act
        var @group = new Group(categoryId, name);

        // Assert
        @group.Id.Should().Be(categoryId);
        @group.Name.Should().Be(name);
    }
}
