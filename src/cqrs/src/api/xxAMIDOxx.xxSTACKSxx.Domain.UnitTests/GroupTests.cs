using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain.ValueObjects;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests;

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
