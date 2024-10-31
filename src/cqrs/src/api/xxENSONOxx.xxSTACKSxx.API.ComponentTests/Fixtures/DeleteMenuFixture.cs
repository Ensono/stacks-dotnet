using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class DeleteMenuFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions) : ApiClientFixture(jwtBearerAuthenticationOptions)
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

    public void GivenAMenuExists()
    {
        repository.GetByIdAsync(id: Arg.Any<Guid>())
            .Returns(new Fixture().Create<Menu>());

        repository.DeleteAsync(Arg.Any<Guid>()).Returns(true);
    }

    public void GivenAMenuDoesNotExist()
    {
        repository.GetByIdAsync(id: Arg.Any<Guid>())
            .Returns((Menu)null!);
    }

    public async Task WhenTheMenuDeletionIsSubmitted()
    {
        await DeleteMenu(Guid.NewGuid().ToString());
    }

    public void ThenTheMenuIsRemovedFromDatabase()
    {
        repository.Received(1).DeleteAsync(Arg.Any<Guid>());
    }

    public void ThenNoMenuIsRemovedFromDatabase()
    {
        repository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
    }

    public void ThenAMenuDeletedEventIsRaised()
    {
        applicationEventPublisher.Received(1).PublishAsync(Arg.Any<MenuDeletedEvent>());
    }

    public void ThenAMenuDeletedEventIsNotRaised()
    {
        applicationEventPublisher.DidNotReceive().PublishAsync(Arg.Any<MenuDeletedEvent>());
    }
}
