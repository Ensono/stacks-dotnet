using System;
using Amido.Stacks.Core.Exceptions;
using FluentAssertions;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests;

public class ExceptionsTests
{
    [Fact]
    public void CategoryAlreadyExistsException_DerivesFrom_DomainExceptionBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(CategoryAlreadyExistsException).Should().BeDerivedFrom<DomainExceptionBase>();
    }

    [Fact]
    public void CategoryAlreadyExistsException_Throws_CorrespondingExceptionWithExpectedMessage()
    {
        // Arrange
        var expectedMessage = "This is the expected message";
        // Act
        Action act = () => CategoryAlreadyExistsException.Raise(Guid.Empty, "testCategoryName", expectedMessage);

        // Assert
        act
            .Should()
            .Throw<CategoryAlreadyExistsException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void CategoryAlreadyExistsException_Throws_CorrespondingExceptionWithExpectedMessage_WhenMessageIsNull()
    {
        // Arrange
        // Act
        Action act = () => CategoryAlreadyExistsException.Raise(Guid.Empty, "testCategoryName", null);

        // Assert
        act
            .Should()
            .Throw<CategoryAlreadyExistsException>()
            .WithMessage("A category with name 'testCategoryName' already exists in the menu '00000000-0000-0000-0000-000000000000'.");
    }

    [Fact]
    public void CategoryDoesNotExistException_DerivesFrom_DomainExceptionBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(CategoryDoesNotExistException).Should().BeDerivedFrom<DomainExceptionBase>();
    }

    [Fact]
    public void CategoryDoesNotExistException_Throws_CorrespondingExceptionWithExpectedMessage()
    {
        // Arrange
        var expectedMessage = "This is the expected message";
        // Act
        Action act = () => CategoryDoesNotExistException.Raise(Guid.Empty, Guid.Empty, expectedMessage);

        // Assert
        act
            .Should()
            .Throw<CategoryDoesNotExistException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void CategoryDoesNotExistException_Throws_CorrespondingExceptionWithExpectedMessage_WhenMessageIsNull()
    {
        // Arrange
        // Act
        Action act = () => CategoryDoesNotExistException.Raise(Guid.Empty, Guid.Empty, null);

        // Assert
        act
            .Should()
            .Throw<CategoryDoesNotExistException>()
            .WithMessage("A category with id '00000000-0000-0000-0000-000000000000' does not exist in the menu '00000000-0000-0000-0000-000000000000'.");
    }

    [Fact]
    public void MenuItemAlreadyExistsException_DerivesFrom_DomainExceptionBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemAlreadyExistsException).Should().BeDerivedFrom<DomainExceptionBase>();
    }

    [Fact]
    public void MenuItemAlreadyExistsException_Throws_CorrespondingExceptionWithExpectedMessage()
    {
        // Arrange
        var expectedMessage = "This is the expected message";
        // Act
        Action act = () => MenuItemAlreadyExistsException.Raise(Guid.Empty, "menuItemName", expectedMessage);

        // Assert
        act
            .Should()
            .Throw<MenuItemAlreadyExistsException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void MenuItemAlreadyExistsException_Throws_CorrespondingExceptionWithExpectedMessage_WhenMessageIsNull()
    {
        // Arrange
        // Act
        Action act = () => MenuItemAlreadyExistsException.Raise(Guid.Empty, "menuItemName", null);

        // Assert
        act
            .Should()
            .Throw<MenuItemAlreadyExistsException>()
            .WithMessage("The item menuItemName already exist in the category '00000000-0000-0000-0000-000000000000'.");
    }

    [Fact]
    public void MenuItemDoesNotExistException_DerivesFrom_DomainExceptionBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemDoesNotExistException).Should().BeDerivedFrom<DomainExceptionBase>();
    }

    [Fact]
    public void MenuItemDoesNotExistException_Throws_CorrespondingExceptionWithExpectedMessage()
    {
        // Arrange
        var expectedMessage = "This is the expected message";
        // Act
        Action act = () => MenuItemDoesNotExistException.Raise(Guid.Empty, Guid.Empty, expectedMessage);

        // Assert
        act
            .Should()
            .Throw<MenuItemDoesNotExistException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void MenuItemDoesNotExistException_Throws_CorrespondingExceptionWithExpectedMessage_WhenMessageIsNull()
    {
        // Arrange
        // Act
        Action act = () => MenuItemDoesNotExistException.Raise(Guid.Empty, Guid.Empty, null);

        // Assert
        act
            .Should()
            .Throw<MenuItemDoesNotExistException>()
            .WithMessage("The item 00000000-0000-0000-0000-000000000000 does not exist in the category '00000000-0000-0000-0000-000000000000'.");
    }

    [Fact]
    public void MenuItemPriceMustNotBeZeroException_DerivesFrom_DomainExceptionBase()
    {
        // Arrange
        // Act
        // Assert
        typeof(MenuItemPriceMustNotBeZeroException).Should().BeDerivedFrom<DomainExceptionBase>();
    }

    [Fact]
    public void MenuItemPriceMustNotBeZeroException_Throws_CorrespondingExceptionWithExpectedMessage()
    {
        // Arrange
        var expectedMessage = "This is the expected message";
        // Act
        Action act = () => MenuItemPriceMustNotBeZeroException.Raise("itemName", expectedMessage);

        // Assert
        act
            .Should()
            .Throw<MenuItemPriceMustNotBeZeroException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void MenuItemPriceMustNotBeZeroException_Throws_CorrespondingExceptionWithExpectedMessage_WhenMessageIsNull()
    {
        // Arrange
        // Act
        Action act = () => MenuItemPriceMustNotBeZeroException.Raise("itemName", null);

        // Assert
        act
            .Should()
            .Throw<MenuItemPriceMustNotBeZeroException>()
            .WithMessage("The price for the item itemName had price as zero. A price must be provided.");
    }
}
