using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class CreateMenuFixture(
    CreateMenuRequest newMenu,
    IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    IMenuRepository repository = null!;
    IApplicationEventPublisher applicationEventPublisher = null!;

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        base.RegisterDependencies(collection);

        repository = Substitute.For<IMenuRepository>();
        applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

        collection.AddTransient(_ => repository);
        collection.AddTransient(_ => applicationEventPublisher);
    }

    internal void GivenAInvalidMenu()
    {
        newMenu.Name = null;
        newMenu.Description = null;
    }

    internal void GivenAMenuDoesNotExist()
    {
        repository.GetByIdAsync(id: Arg.Any<Guid>())
            .Returns((Domain.Menu)null!);
    }

    internal async Task WhenTheMenuCreationIsSubmitted()
    {
        await CreateMenu(newMenu);
    }

    internal void ThenTheMenuIsSubmittedToDatabase()
    {
        repository.Received(1).SaveAsync(Arg.Is<Domain.Menu>(menu => menu.Name == newMenu.Name));
    }

    internal void ThenTheMenuIsNotSubmittedToDatabase()
    {
        repository.DidNotReceive().SaveAsync(Arg.Any<Domain.Menu>());
    }

    internal void ThenAMenuCreatedEventIsRaised()
    {
        applicationEventPublisher.Received(1).PublishAsync(Arg.Any<MenuCreatedEvent>());
    }

    internal void ThenAMenuCreatedEventIsNotRaised()
    {
        applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<MenuCreatedEvent>());
    }
}
