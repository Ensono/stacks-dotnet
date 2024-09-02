using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.Abstractions;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB;
using xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class MenuRepositoryAutoData() : AutoDataAttribute(Customizations)
{
    private static IFixture Customizations()
    {
        var settings = Shared.Testing.Settings.Configuration.For<CosmosDbConfiguration>("CosmosDB");

        IFixture fixture = new Fixture();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<ILogger<CosmosDbDocumentStorage<Menu>>>(() => new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
        fixture.Register(() => settings.AsOption());
        fixture.Register<IDocumentStorage<Menu>>(() => fixture.Create<CosmosDbDocumentStorage<Menu>>());

        return fixture;
    }
}
