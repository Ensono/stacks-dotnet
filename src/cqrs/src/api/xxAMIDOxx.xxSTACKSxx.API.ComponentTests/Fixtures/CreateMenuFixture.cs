using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.Abstractions;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class CreateMenuFixture(
    CreateMenuRequest newMenu,
    IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiClientFixture(jwtBearerAuthenticationOptions)
{
    IMenuRepository repository;
    IApplicationEventPublisher applicationEventPublisher;

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        base.RegisterDependencies(collection);

        // Mocked external dependencies, the setup should
        // come later according to the scenarios
        repository = Substitute.For<IMenuRepository>();
        var dou = Substitute.For<IDocumentSearch<Menu>>();
        applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

        collection.AddTransient(_ => dou);
        collection.AddTransient(_ => repository);
        collection.AddTransient(_ => applicationEventPublisher);
    }


    /****** GIVEN ******************************************************/

    internal void GivenAInvalidMenu()
    {
        newMenu.Name = null;
        newMenu.Description = null;
    }


    internal void GivenAMenuDoesNotExist()
    {
        repository.GetByIdAsync(id: Arg.Any<Guid>())
            .Returns((Domain.Menu)null);
    }


    /****** WHEN ******************************************************/

    internal async Task WhenTheMenuCreationIsSubmitted()
    {
        await CreateMenu(newMenu);
    }

    /****** THEN ******************************************************/

    internal void ThenGetMenuByIdIsCalled()
    {
        repository.Received(1).GetByIdAsync(Arg.Any<Guid>());
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
