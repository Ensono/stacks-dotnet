using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

public class CreateMenuFixture : ApiClientFixture
{
    readonly CreateMenuRequest newMenu;
    IMenuRepository repository;
    IApplicationEventPublisher applicationEventPublisher;

    public CreateMenuFixture(CreateMenuRequest newMenu, IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
        : base(jwtBearerAuthenticationOptions)
    {
        this.newMenu = newMenu;
    }

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        base.RegisterDependencies(collection);

        // Mocked external dependencies, the setup should
        // come later according to the scenarios
        repository = Substitute.For<IMenuRepository>();
        applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

        collection.AddTransient(IoC => repository);
        collection.AddTransient(IoC => applicationEventPublisher);
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
