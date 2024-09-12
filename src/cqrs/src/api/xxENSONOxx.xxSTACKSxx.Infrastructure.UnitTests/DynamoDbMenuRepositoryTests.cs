#if DynamoDb
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Entities;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.UnitTests;

public class DynamoDbMenuRepositoryTests
{
    private readonly DynamoDbMenuRepository repository;
    private readonly IDynamoDbObjectStorage<Menu> fakeMenuRepository;
    private Menu menu;
    private string partitionKey;

    public DynamoDbMenuRepositoryTests()
    {
        var id = Guid.NewGuid();
        partitionKey = id.ToString();
        menu = new Menu(id, "testName", Guid.Empty, "testDescription", true, new List<Category>());
        fakeMenuRepository = SetupFakeMenuRepository();
        repository = new DynamoDbMenuRepository(fakeMenuRepository);
    }

    [Fact]
    public void DynamoDbMenuRepository_Should_ImplementIMenuRepository()
    {
        // Arrange
        // Act
        // Assert
        typeof(DynamoDbMenuRepository).Should().Implement<IMenuRepository>();
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        // Arrange
        // Act
        var result = await repository.GetByIdAsync(Guid.Empty);

        // Assert
        await fakeMenuRepository.Received().GetByIdAsync(Arg.Any<string>());

        result.Should().BeOfType<Menu>();
        result.Should().Be(menu);
    }

    [Fact]
    public async Task SaveAsync()
    {
        // Arrange
        // Act
        var result = await repository.SaveAsync(menu);

        // Assert
        await fakeMenuRepository.Received().SaveAsync(partitionKey, menu);

        result.Should().Be(true);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        // Act
        var result = await repository.DeleteAsync(Guid.Empty);

        // Assert
        await fakeMenuRepository.Received().DeleteAsync(Arg.Any<string>());

        result.Should().Be(true);
    }

    private IDynamoDbObjectStorage<Menu> SetupFakeMenuRepository()
    {
        var menuRepository = Substitute.For<IDynamoDbObjectStorage<Menu>>();
        var fakeTypeResponse = new OperationResult<Menu>(true, menu, new Dictionary<string, string>());
        var fakeNonTypeResponse = new OperationResult(true, new Dictionary<string, string>());

        menuRepository.GetByIdAsync(Arg.Any<string>())
            .Returns(Task.FromResult(fakeTypeResponse));
        menuRepository.SaveAsync(Arg.Any<string>(), Arg.Any<Menu>())
            .Returns(Task.FromResult(fakeTypeResponse));
        menuRepository.DeleteAsync(Arg.Any<string>())
            .Returns(Task.FromResult(fakeNonTypeResponse));

        return menuRepository;
    }
}
#endif
