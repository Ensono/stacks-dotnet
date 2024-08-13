using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;

namespace xxAMIDOxx.xxSTACKSxx.API.UnitTests.Models;

public class SearchMenuResponseTests
{
    [Fact]
    public void Size_Should_ReturnInt()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuResponse)
            .Properties()
            .First(x => x.Name == "Size")
            .Should()
            .Return<int>();
    }

    [Fact]
    public void Offset_Should_ReturnInt()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuResponse)
            .Properties()
            .First(x => x.Name == "Offset")
            .Should()
            .Return<int>();
    }

    [Fact]
    public void Results_Should_ReturnList()
    {
        // Arrange
        // Act
        // Assert
        typeof(SearchMenuResponse)
            .Properties()
            .First(x => x.Name == "Results")
            .Should()
            .Return<List<SearchMenuResponseItem>>();
    }
}
