using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Models;

public class ResourceCreatedResponseTests
{
    [Fact]
    public void Id_Should_ReturnGuid()
    {
        // Arrange
        // Act
        // Assert
        typeof(ResourceCreatedResponse)
            .Properties()
            .First(x => x.Name == "Id")
            .Should()
            .Return<Guid>();
    }
}
