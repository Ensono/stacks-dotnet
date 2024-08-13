using System;
using System.Collections.Generic;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.Data.Documents.Abstractions;
using AutoFixture;
using AutoFixture.Xunit2;
using NSubstitute;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
using xxAMIDOxx.xxSTACKSxx.Common.Exceptions;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions;
using Query = xxAMIDOxx.xxSTACKSxx.CQRS.Queries;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.UnitTests;

/// <summary>
/// Series of tests for command handlers
/// </summary>
[Trait("TestType", "UnitTests")]
public class HandlerTests
{
    private Fixture fixture;
    private IMenuRepository menuRepo;
    private IApplicationEventPublisher eventPublisher;
    private IDocumentSearch<Domain.Menu> storage;

    public HandlerTests()
    {
        fixture = new Fixture();
        fixture.Register(() => Substitute.For<IOperationContext>());
        fixture.Register(() => Substitute.For<IMenuRepository>());
        fixture.Register(() => Substitute.For<IApplicationEventPublisher>());
        fixture.Register(() => Substitute.For<IDocumentSearch<Domain.Menu>>());

        menuRepo = fixture.Create<IMenuRepository>();
        eventPublisher = fixture.Create<IApplicationEventPublisher>();
        storage = fixture.Create<IDocumentSearch<Domain.Menu>>();
    }

    #region CREATE

    [Theory, AutoData]
    public async void CreateMenuCommandHandler_HandleAsync(CreateMenu command)
    {
        // Arrange
        var handler = new CreateMenuCommandHandler(menuRepo, eventPublisher);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        await menuRepo.Received(1).SaveAsync(Arg.Any<Domain.Menu>());
        await eventPublisher.Received(1).PublishAsync(Arg.Any<IApplicationEvent>());

        result.ShouldBeOfType<Guid>();
    }

    [Theory, AutoData]
    public async void CreateCategoryCommandHandler_HandleAsync(Domain.Menu menu, CreateCategory command)
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(menuRepo, eventPublisher);

        // Act
        var result = await handler.HandleCommandAsync(menu, command);

        // Assert
        result.ShouldBeOfType<Guid>();
    }

    [Theory, AutoData]
    public async void CreateMenuItemCommandHandler_HandleAsync(Domain.Menu menu, CreateMenuItem command)
    {
        // Arrange
        var handler = new CreateMenuItemCommandHandler(menuRepo, eventPublisher);
        command.CategoryId = menu.Categories[0].Id;

        // Act
        var result = await handler.HandleCommandAsync(menu, command);

        // Assert
        result.ShouldBeOfType<Guid>();
    }

    #endregion

    #region DELETE

    [Theory, AutoData]
    public async void DeleteMenuCommandHandler_HandleAsync(Domain.Menu menu, DeleteMenu command)
    {
        // Arrange
        menuRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(menu);
        menuRepo.DeleteAsync(Arg.Any<Guid>()).Returns(true);

        var handler = new DeleteMenuCommandHandler(menuRepo, eventPublisher);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        await menuRepo.Received(1).DeleteAsync(Arg.Any<Guid>());
        await eventPublisher.Received(1).PublishAsync(Arg.Any<IApplicationEvent>());

        result.ShouldBeOfType<bool>();
        result.ShouldBeTrue();
    }

    [Theory, AutoData]
    public async void DeleteMenuCommandHandler_HandleAsync_MenuMissing_ShouldThrow(DeleteMenu command)
    {
        // Arrange
        var handler = fixture.Create<DeleteMenuCommandHandler>();

        // Act
        // Assert
        await handler.HandleAsync(command).ShouldThrowAsync<MenuDoesNotExistException>();
        await menuRepo.Received(0).DeleteAsync(Arg.Any<Guid>());
        await eventPublisher.Received(0).PublishAsync(Arg.Any<IApplicationEvent>());
    }

    [Theory, AutoData]
    public async void DeleteMenuCommandHandler_HandleAsync_NotSuccessful_ShouldThrow(Domain.Menu menu, DeleteMenu command)
    {
        // Arrange
        menuRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(menu);
        menuRepo.DeleteAsync(Arg.Any<Guid>()).Returns(false);
        var handler = new DeleteMenuCommandHandler(menuRepo, eventPublisher);

        // Act
        // Assert
        await handler.HandleAsync(command).ShouldThrowAsync<OperationFailedException>();
        await menuRepo.Received(1).DeleteAsync(Arg.Any<Guid>());
        await eventPublisher.Received(0).PublishAsync(Arg.Any<IApplicationEvent>());
    }

    #endregion

    #region UPDATE

    [Theory, AutoData]
    public async void UpdateMenuCommandHandler_HandleAsync(Domain.Menu menu, UpdateMenu command)
    {
        // Arrange
        var handler = new UpdateMenuCommandHandler(menuRepo, eventPublisher);

        // Act
        var result = await handler.HandleCommandAsync(menu, command);

        // Assert
        result.ShouldBeOfType<bool>();
        result.ShouldBe(true);
    }

    [Theory, AutoData]
    public async void UpdateCategoryCommandHandler_HandleAsync(Domain.Menu menu, UpdateCategory command)
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(menuRepo, eventPublisher);
        command.CategoryId = menu.Categories[0].Id;

        // Act
        var result = await handler.HandleCommandAsync(menu, command);

        // Assert
        result.ShouldBeOfType<bool>();
        result.ShouldBe(true);
    }

    [Theory, AutoData]
    public async void UpdateMenuItemCommandHandler_HandleAsync(Domain.Menu menu, UpdateMenuItem command)
    {
        // Arrange
        var handler = new UpdateMenuItemCommandHandler(menuRepo, eventPublisher);
        command.CategoryId = menu.Categories[0].Id;
        command.MenuItemId = menu.Categories[0].Items[0].Id;


        // Act
        var result = await handler.HandleCommandAsync(menu, command);

        // Assert
        result.ShouldBeOfType<bool>();
        result.ShouldBe(true);
    }

    [Theory, AutoData]
    public async void UpdateCategoryCommandHandler_HandleAsync_NoCategory_ShouldThrow(Domain.Menu menu, UpdateCategory command)
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(menuRepo, eventPublisher);

        // Act
        // Assert
        await Should.ThrowAsync<CategoryDoesNotExistException>(async () => await handler.HandleCommandAsync(menu, command));
    }

    [Theory, AutoData]
    public async void UpdateMenuItemCommandHandler_HandleAsync_NoMenuItem_ShouldThrow(Domain.Menu menu, UpdateMenuItem command)
    {
        // Arrange
        var handler = new UpdateMenuItemCommandHandler(menuRepo, eventPublisher);
        command.CategoryId = menu.Categories[0].Id;

        // Act
        // Assert
        await Should.ThrowAsync<MenuItemDoesNotExistException>(async () => await handler.HandleCommandAsync(menu, command));
    }

    #endregion

    #region QUERIES

    [Theory, AutoData]
    public async void GetMenuByIdQueryHandler_ExecuteAsync(Domain.Menu menu, GetMenuById criteria)
    {
        // Arrange
        menuRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(menu);
        var handler = new GetMenuByIdQueryHandler(menuRepo);

        // Act
        var result = await handler.ExecuteAsync(criteria);

        // Assert
        await menuRepo.Received(1).GetByIdAsync(Arg.Any<Guid>());
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Menu>();
    }

    [Theory, AutoData]
    public async void GetMenuByIdQueryHandler_ExecuteAsync_NoMenu_ReturnNull(GetMenuById criteria)
    {
        // Arrange
        var handler = new GetMenuByIdQueryHandler(menuRepo);

        // Act
        var result = await handler.ExecuteAsync(criteria);

        // Assert
        await menuRepo.Received(1).GetByIdAsync(Arg.Any<Guid>());
        result.ShouldBeNull();
    }

    [Theory, AutoData]
    public async void SearchMenuQueryHandler_ExecuteAsync(SearchMenu criteria, OperationResult<IEnumerable<Domain.Menu>> operationResult)
    {
        // Arrange
        storage.Search(
            Arg.Any<System.Linq.Expressions.Expression<Func<Domain.Menu, bool>>>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>())
        .Returns(operationResult);

        var handler = new SearchMenuQueryHandler(storage);

        // Act
        var result = await handler.ExecuteAsync(criteria);

        // Assert
        await storage.Received(1).Search(
            Arg.Any<System.Linq.Expressions.Expression<Func<Domain.Menu, bool>>>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>());

        result.ShouldBeOfType<SearchMenuResult>();
    }

    [Fact]
    public async void SearchMenuQueryHandler_ExecuteAsync_NoCriteria_ShouldThrow()
    {
        // Arrange
        var handler = new SearchMenuQueryHandler(storage);

        // Act
        // Assert
        await Should.ThrowAsync<ArgumentException>(async () => await handler.ExecuteAsync(null));
    }

    [Theory, AutoData]
    public async void SearchMenuQueryHandler_ExecuteAsync_NotSuccessful(SearchMenu criteria)
    {
        // Arrange
        var operationResult = new OperationResult<IEnumerable<Domain.Menu>>(false, null, null);

        storage.Search(
            Arg.Any<System.Linq.Expressions.Expression<Func<Domain.Menu, bool>>>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>())
        .Returns(operationResult);

        var handler = new SearchMenuQueryHandler(storage);

        // Act
        var result = await handler.ExecuteAsync(criteria);

        // Assert
        await storage.Received(1).Search(
            Arg.Any<System.Linq.Expressions.Expression<Func<Domain.Menu, bool>>>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>());

        result.ShouldBeOfType<SearchMenuResult>();
        result.Results.ShouldBeNull();
    }

    #endregion
}
