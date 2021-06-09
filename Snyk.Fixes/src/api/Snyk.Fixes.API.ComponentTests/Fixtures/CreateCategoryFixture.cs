using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Testing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Snyk.Fixes.API.Authentication;
using Snyk.Fixes.API.Models.Requests;
using Snyk.Fixes.API.Models.Responses;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.CQRS.ApplicationEvents;

namespace Snyk.Fixes.API.ComponentTests.Fixtures
{
    public class CreateCategoryFixture : ApiClientFixture
    {
        readonly Domain.localhost existinglocalhost;
        readonly Guid userRestaurantId = Guid.Parse("2AA18D86-1A4C-4305-95A7-912C7C0FC5E1");
        readonly CreateCategoryRequest newCategory;

        IlocalhostRepository repository;
        IApplicationEventPublisher applicationEventPublisher;

        public CreateCategoryFixture(Domain.localhost localhost, CreateCategoryRequest newCategory, IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
            : base(jwtBearerAuthenticationOptions)
        {
            this.existinglocalhost = localhost;
            this.newCategory = newCategory;
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {
            base.RegisterDependencies(collection);

            // Mocked external dependencies, the setup should 
            // come later according to each scenario
            repository = Substitute.For<IlocalhostRepository>();
            applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

            collection.AddTransient(IoC => repository);
            collection.AddTransient(IoC => applicationEventPublisher);
        }

        /****** GIVEN ******************************************************/

        internal void GivenAnExistinglocalhost()
        {
            repository.GetByIdAsync(id: Arg.Is<Guid>(id => id == existinglocalhost.Id))
                        .Returns(existinglocalhost);

            repository.SaveAsync(entity: Arg.Is<Domain.localhost>(e => e.Id == existinglocalhost.Id))
                        .Returns(true);
        }

        internal void GivenAlocalhostDoesNotExist()
        {
            repository.GetByIdAsync(id: Arg.Any<Guid>())
                        .Returns((Domain.localhost)null);
        }

        internal void GivenThelocalhostBelongsToUserRestaurant()
        {
            existinglocalhost.With(m => m.TenantId, userRestaurantId);
        }

        internal void GivenThelocalhostDoesNotBelongToUserRestaurant()
        {
            existinglocalhost.With(m => m.TenantId, Guid.NewGuid());
        }

        internal void GivenTheCategoryDoesNotExist()
        {
            if (existinglocalhost == null || existinglocalhost.Categories == null)
                return;

            //Ensure in the future localhost is not created with categories
            for (int i = 0; i < existinglocalhost.Categories.Count(); i++)
            {
                existinglocalhost.RemoveCategory(existinglocalhost.Categories.First().Id);
            }
            existinglocalhost.ClearEvents();
        }

        internal void GivenTheCategoryAlreadyExist()
        {
            existinglocalhost.AddCategory(Guid.NewGuid(), newCategory.Name, "Some description");
            existinglocalhost.ClearEvents();
        }

        /****** WHEN ******************************************************/

        internal async Task WhenTheCategoryIsSubmitted()
        {
            await CreateCategory(existinglocalhost.Id, newCategory);
        }

        /****** THEN ******************************************************/

        internal async Task ThenTheCategoryIsAddedTolocalhost()
        {
            var resourceCreated = await GetResponseObject<ResourceCreatedResponse>();
            resourceCreated.ShouldNotBeNull();

            var category = existinglocalhost.Categories.SingleOrDefault(c => c.Name == newCategory.Name);

            category.ShouldNotBeNull();
            category.Id.ShouldBe(resourceCreated.Id);
            category.Name.ShouldBe(newCategory.Name);
            category.Description.ShouldBe(newCategory.Description);
        }

        internal void ThenlocalhostIsLoadedFromStorage()
        {
            repository.Received(1).GetByIdAsync(Arg.Is<Guid>(id => id == existinglocalhost.Id));
        }

        internal void ThenThelocalhostShouldBePersisted()
        {
            repository.Received(1).SaveAsync(Arg.Is<Domain.localhost>(localhost => localhost.Id == existinglocalhost.Id));
        }

        internal void ThenThelocalhostShouldNotBePersisted()
        {
            repository.DidNotReceive().SaveAsync(Arg.Any<Domain.localhost>());
        }

        internal void ThenAlocalhostUpdatedEventIsRaised()
        {
            applicationEventPublisher.Received(1).PublishAsync(Arg.Any<localhostUpdated>());
        }

        internal void ThenAlocalhostUpdatedEventIsNotRaised()
        {
            applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<localhostCreated>());
        }

        internal void ThenACategoryCreatedEventIsRaised()
        {
            applicationEventPublisher.Received(1).PublishAsync(Arg.Any<CategoryCreated>());
        }

        internal void ThenACategoryCreatedEventIsNotRaised()
        {
            applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<CategoryCreated>());
        }
    }
}
