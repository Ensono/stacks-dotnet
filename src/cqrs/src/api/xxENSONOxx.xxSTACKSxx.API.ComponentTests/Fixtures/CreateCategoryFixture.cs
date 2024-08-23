using System;
using System.Linq;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Testing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.API.Models.Responses;
using xxENSONOxx.xxSTACKSxx.Application.Integration;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class CreateCategoryFixture(
    Domain.Menu existingMenu,
    CreateCategoryRequest newCategory,
    IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    readonly Guid userRestaurantId = Guid.Parse("2AA18D86-1A4C-4305-95A7-912C7C0FC5E1");

    IMenuRepository repository;
    IApplicationEventPublisher applicationEventPublisher;

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        base.RegisterDependencies(collection);

        // Mocked external dependencies, the setup should
        // come later according to each scenario
        repository = Substitute.For<IMenuRepository>();
        applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

        collection.AddTransient(IoC => repository);
        collection.AddTransient(IoC => applicationEventPublisher);
    }

    /****** GIVEN ******************************************************/

    internal void GivenAnExistingMenu()
    {
        repository.GetByIdAsync(id: Arg.Is<Guid>(id => id == existingMenu.Id))
            .Returns(existingMenu);

        repository.SaveAsync(entity: Arg.Is<Domain.Menu>(e => e.Id == existingMenu.Id))
            .Returns(true);
    }

    internal void GivenAMenuDoesNotExist()
    {
        repository.GetByIdAsync(id: Arg.Any<Guid>())
            .Returns((Domain.Menu)null);
    }

    internal void GivenTheMenuBelongsToUserRestaurant()
    {
        existingMenu.With(m => m.TenantId, userRestaurantId);
    }

    internal void GivenTheMenuDoesNotBelongToUserRestaurant()
    {
        existingMenu.With(m => m.TenantId, Guid.NewGuid());
    }

    internal void GivenTheCategoryDoesNotExist()
    {
        if (existingMenu == null || existingMenu.Categories == null)
            return;

        //Ensure in the future menu is not created with categories
        for (int i = 0; i < existingMenu.Categories.Count(); i++)
        {
            existingMenu.RemoveCategory(existingMenu.Categories[0].Id);
        }
        existingMenu.ClearEvents();
    }

    internal void GivenTheCategoryAlreadyExist()
    {
        existingMenu.AddCategory(Guid.NewGuid(), newCategory.Name, "Some description");
        existingMenu.ClearEvents();
    }

    /****** WHEN ******************************************************/

    internal async Task WhenTheCategoryIsSubmitted()
    {
        await CreateCategory(existingMenu.Id, newCategory);
    }

    /****** THEN ******************************************************/

    internal async Task ThenTheCategoryIsAddedToMenu()
    {
        var resourceCreated = await GetResponseObject<ResourceCreatedResponse>();
        resourceCreated.ShouldNotBeNull();

        var category = existingMenu.Categories.SingleOrDefault(c => c.Name == newCategory.Name);

        category.ShouldNotBeNull();
        category.Id.ShouldBe(resourceCreated.Id);
        category.Name.ShouldBe(newCategory.Name);
        category.Description.ShouldBe(newCategory.Description);
    }

    internal void ThenMenuIsLoadedFromStorage()
    {
        repository.Received(1).GetByIdAsync(Arg.Is<Guid>(id => id == existingMenu.Id));
    }

    internal void ThenTheMenuShouldBePersisted()
    {
        repository.Received(1).SaveAsync(Arg.Is<Domain.Menu>(menu => menu.Id == existingMenu.Id));
    }

    internal void ThenTheMenuShouldNotBePersisted()
    {
        repository.DidNotReceive().SaveAsync(Arg.Any<Domain.Menu>());
    }

    internal void ThenAMenuUpdatedEventIsRaised()
    {
        applicationEventPublisher.Received(1).PublishAsync(Arg.Any<MenuUpdatedEvent>());
    }

    internal void ThenAMenuUpdatedEventIsNotRaised()
    {
        applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<MenuCreatedEvent>());
    }

    internal void ThenACategoryCreatedEventIsRaised()
    {
        applicationEventPublisher.Received(1).PublishAsync(Arg.Any<CategoryCreatedEvent>());
    }

    internal void ThenACategoryCreatedEventIsNotRaised()
    {
        applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CategoryCreatedEvent>());
    }
}
