using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;
#if (CosmosDb)
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class MenuRepositoryAutoData() : AutoDataAttribute(Customizations)
{
    private static IFixture Customizations()
    {
#if (CosmosDb)
        var settings = Shared.Testing.Settings.Configuration.For<CosmosDbConfiguration>("CosmosDB");
#endif
        IFixture fixture = new Fixture();

        var loggerFactory = Substitute.For<ILoggerFactory>();
#if (CosmosDb)
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<ILogger<CosmosDbDocumentStorage<Menu>>>(() => new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<IDocumentStorage<Menu>>(() => fixture.Create<CosmosDbDocumentStorage<Menu>>());
#endif
        fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
#if (CosmosDb)
        fixture.Register(() => settings.AsOption());
#endif
        return fixture;
    }
}
