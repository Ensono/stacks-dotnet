using System;
using System.Linq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests;

[Trait("TestType", "UnitTests")]
public class MenuTests
{
    [Theory, AutoData]
    public void Constructor(
        string name,
        string description,
        bool enabled)
    {
        // Arrange
        // Act
        var menu = new Menu(Guid.Empty, name, Guid.Empty, description, enabled);

        // Assert
        menu.Name.Should().Be(name);
        menu.Description.Should().Be(description);
        menu.Enabled.Should().Be(enabled);
    }
    
    [Theory, AutoData]
    public void Update(
        Menu menu,
        string name,
        string description,
        bool enabled)
    {
        // Arrange
        // Act
        menu.Update(name, description, enabled);

        // Assert
        menu.Name.Should().Be(name);
        menu.Description.Should().Be(description);
        menu.Enabled.Should().Be(enabled);
    }    
    
    [Theory, AutoData]
    public void AddCategory(
        Menu menu,
        Guid categoryId,
        string name,
        string description)
    {
        // Arrange
        // Act
        menu.AddCategory(categoryId, name, description);

        // Assert
        menu.Categories.Last().Id.Should().Be(categoryId);
        menu.Categories.Last().Name.Should().Be(name);
        menu.Categories.Last().Description.Should().Be(description);
    }

    [Theory, AutoData]
    public void UpdateCategory(
        Menu menu,
        Guid categoryId,
        string name,
        string description,
        string updatedName,
        string updatedDescription)
    {
        // Arrange
        menu.AddCategory(categoryId, name, description);

        // Act
        menu.UpdateCategory(categoryId, updatedName, updatedDescription);

        // Assert
        menu.Categories.Last().Id.Should().Be(categoryId);
        menu.Categories.Last().Name.Should().Be(updatedName);
        menu.Categories.Last().Description.Should().Be(updatedDescription);
    }

    [Theory, AutoData]
    public void RemoveCategory(
        Menu menu,
        Guid categoryId,
        string name,
        string description)
    {
        // Arrange
        menu.AddCategory(categoryId, name, description);
        var categoriesLength = menu.Categories.Count;
        
        // Assert
        menu.Categories.Should().Contain(category => category.Id == categoryId);

        // Act
        menu.RemoveCategory(categoryId);

        // Assert
        menu.Categories.Should().NotContain(category => category.Id == categoryId);
        menu.Categories.Count.Should().Be(categoriesLength - 1);
    }
    
    [Theory, AutoData]
    public void AddMenuItem(
        Menu menu,
        Guid categoryId,
        string categoryName,
        string categoryDescription,
        Guid menuItemId,
        string menuItemName,
        string menuItemDescription,
        double menuItemPrice,
        bool menuItemAvailable)
    {
        // Arrange
        menu.AddCategory(categoryId, categoryName, categoryDescription);

        // Act
        menu.AddMenuItem(categoryId, menuItemId, menuItemName, menuItemDescription, menuItemPrice, menuItemAvailable);
        
        // Assert
        menu
            .Categories
            .Last()
            .Items
            .Should()
            .Contain(menuItem => menuItem.Id == menuItemId && 
                                 menuItem.Name == menuItemName &&
                                 menuItem.Description == menuItemDescription &&
                                 menuItem.Price == menuItemPrice &&
                                 menuItem.Available == menuItemAvailable);
    }

    [Theory, AutoData]
    public void UpdateMenuItem(
        Menu menu,
        Guid categoryId,
        string categoryName,
        string categoryDescription,
        Guid menuItemId,
        string menuItemName,
        string menuItemDescription,
        double menuItemPrice,
        bool menuItemAvailable,
        string updatedMenuItemName,
        string updatedMenuItemDescription,
        double updatedMenuItemPrice,
        bool updatedMenuItemAvailable)
    {
        // Arrange
        menu.AddCategory(categoryId, categoryName, categoryDescription);
        menu.AddMenuItem(categoryId, menuItemId, menuItemName, menuItemDescription, menuItemPrice, menuItemAvailable);

        // Act
        menu.UpdateMenuItem(categoryId, menuItemId, updatedMenuItemName, updatedMenuItemDescription, updatedMenuItemPrice, updatedMenuItemAvailable);
        
        // Assert
        menu
            .Categories
            .Last()
            .Items
            .Should()
            .NotContain(menuItem => menuItem.Id == menuItemId && 
                                    menuItem.Name == menuItemName && 
                                    menuItem.Description == menuItemDescription && 
                                    menuItem.Price == menuItemPrice && 
                                    menuItem.Available == menuItemAvailable);
        
        menu
            .Categories
            .Last()
            .Items
            .Should()
            .Contain(menuItem => menuItem.Id == menuItemId && 
                                 menuItem.Name == updatedMenuItemName && 
                                 menuItem.Description == updatedMenuItemDescription && 
                                 menuItem.Price == updatedMenuItemPrice && 
                                 menuItem.Available == updatedMenuItemAvailable);
    }

    [Theory, AutoData]
    public void RemoveMenuItem(
        Menu menu,
        Guid categoryId,
        string categoryName,
        string categoryDescription,
        Guid menuItemId,
        string menuItemName,
        string menuItemDescription,
        double menuItemPrice,
        bool menuItemAvailable)
    {
        // Arrange
        menu.AddCategory(categoryId, categoryName, categoryDescription);
        menu.AddMenuItem(categoryId, menuItemId, menuItemName, menuItemDescription, menuItemPrice, menuItemAvailable);

        // Assert
        menu
            .Categories
            .Last()
            .Items
            .Should()
            .Contain(menuItem => menuItem.Id == menuItemId &&
                                 menuItem.Name == menuItemName &&
                                 menuItem.Description == menuItemDescription &&
                                 menuItem.Price == menuItemPrice &&
                                 menuItem.Available == menuItemAvailable);

        // Act
        menu.RemoveMenuItem(categoryId, menuItemId);

        // Assert
        menu
            .Categories
            .Last()
            .Items
            .Should()
            .NotContain(menuItem => menuItem.Id == menuItemId &&
                                    menuItem.Name == menuItemName &&
                                    menuItem.Description == menuItemDescription &&
                                    menuItem.Price == menuItemPrice &&
                                    menuItem.Available == menuItemAvailable);
    }
}
