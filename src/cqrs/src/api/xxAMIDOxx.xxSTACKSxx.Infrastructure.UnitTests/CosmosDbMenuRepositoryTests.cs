using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.Data.Documents.Abstractions;
using FluentAssertions;
using NSubstitute;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.UnitTests;

public class CosmosDbMenuRepositoryTests
{
    private readonly CosmosDbMenuRepository repository;
    private readonly IDocumentStorage<Menu> fakeMenuRepository;
    private Menu menu;

    public CosmosDbMenuRepositoryTests()
    {
        menu = new Menu(Guid.Empty, "testName", Guid.Empty, "testDescription", true, new List<Category>());
        fakeMenuRepository = SetupFakeMenuRepository();
        repository = new CosmosDbMenuRepository(fakeMenuRepository);
    }

    [Fact]
    public void CosmosDbMenuRepository_Should_ImplementIMenuRepository()
    {
        // Arrange
        // Act
        // Assert
        typeof(CosmosDbMenuRepository).Should().Implement<IMenuRepository>();
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        // Arrange
        // Act
        var result = await repository.GetByIdAsync(Guid.Empty);

        // Assert
        await fakeMenuRepository.Received().GetByIdAsync(Arg.Any<string>(), Arg.Any<string>());

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
        await fakeMenuRepository.Received().SaveAsync(Arg.Any<string>(), Arg.Any<string>(), menu, Arg.Any<string>());

        result.Should().Be(true);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        // Act
        var result = await repository.DeleteAsync(Guid.Empty);

        // Assert
        await fakeMenuRepository.Received().DeleteAsync(Arg.Any<string>(), Arg.Any<string>());

        result.Should().Be(true);
    }

    private IDocumentStorage<Menu> SetupFakeMenuRepository()
    {
        var menuRepository = Substitute.For<IDocumentStorage<Domain.Menu>>();
        var fakeTypeResponse = new OperationResult<Menu>(true, menu, new Dictionary<string, string>());
        var fakeNonTypeResponse = new OperationResult(true, new Dictionary<string, string>());

        menuRepository.GetByIdAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult(fakeTypeResponse));
        menuRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), menu, Arg.Any<string>())
            .Returns(Task.FromResult(fakeTypeResponse));
        menuRepository.DeleteAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult(fakeNonTypeResponse));

        return menuRepository;
    }
}
