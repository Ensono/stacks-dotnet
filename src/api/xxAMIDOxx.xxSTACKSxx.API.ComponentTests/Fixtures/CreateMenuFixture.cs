﻿using System;
using System.Net;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.Models;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;
using xxAMIDOxx.xxSTACKSxx.Integration;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures
{
    public class CreateMenuFixture : ApiClientFixture
    {
        CreateOrUpdateMenu newMenu;
        IMenuRepository repository;
        IApplicationEventPublisher applicationEventPublisher;

        public CreateMenuFixture(CreateOrUpdateMenu newMenu)
        {
            this.newMenu = newMenu;
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {
            DependencyRegistration.ConfigureStaticServices(collection);

            // Mocked external dependencies, the setup should 
            // come later according to the scenarios
            repository = Substitute.For<IMenuRepository>();
            applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

            collection.AddTransient(IoC => repository);
            collection.AddTransient(IoC => applicationEventPublisher);
        }

        internal void GivenAValidMenu()
        {
            // Don't need to do anything here assuming the
            // newMenu auto generated by AutoFixture is valid
        }
        internal void GivenAInvalidMenu()
        {
            newMenu.Name = null;
            newMenu.Description = null;
        }


        internal void GivenTheMenuDoesNotExist()
        {
            repository.GetByIdAsync(id: Arg.Any<Guid>())
                        .Returns((Domain.Menu)null);
        }

        internal async Task WhenTheMenuCreationIsSubmitted()
        {
            await CreateMenu(newMenu);
        }

        internal void ThenASuccessfulResponseIsReturned()
        {
            //Assert.True(LastResponse.IsSuccessStatusCode);
            LastResponse.IsSuccessStatusCode.ShouldBeTrue();
        }

        internal void ThenAFailureResponseIsReturned()
        {
            //Assert.False(LastResponse.IsSuccessStatusCode);
            LastResponse.IsSuccessStatusCode.ShouldBeFalse();
        }

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

        internal void ThenAForbiddenResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }
    }
}
