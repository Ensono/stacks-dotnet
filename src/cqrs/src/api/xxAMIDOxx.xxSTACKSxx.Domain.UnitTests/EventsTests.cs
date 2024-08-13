using Amido.Stacks.Domain.Events;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain.Events;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests;

public class EventsTests
{
    [Fact]
    public void CategoryChanged()
    {
        // Arrange
        // Act
        // Assert
        typeof(CategoryChanged).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void CategoryCreated()
    {
        // Arrange
        // Act
        // Assert
        typeof(CategoryCreated).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void CategoryRemoved()
    {
        // Arrange
        // Act
        // Assert
        typeof(CategoryRemoved).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void MenuChanged()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuChanged).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void MenuCreated()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuCreated).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void MenuItemChanged()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemChanged).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void MenuItemCreated()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemCreated).Should().Implement<IDomainEvent>();
    }

    [Fact]
    public void MenuItemRemoved()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemRemoved).Should().Implement<IDomainEvent>();
    }
}
