using AutoFixture;
using AutoFixture.Xunit2;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;
#if (CosmosDb)
using Microsoft.Extensions.Logging;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
#endif

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class MenuRepositoryAutoData() : AutoDataAttribute(Customizations)
{
    private static IFixture Customizations()
    {
        IFixture fixture = new Fixture();

#if (CosmosDb)
        var settings = Configuration.For<CosmosDbConfiguration>("CosmosDB");
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<ILogger<CosmosDbDocumentStorage<Menu>>>(() => new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
        fixture.Register<IDocumentStorage<Menu>>(() => fixture.Create<CosmosDbDocumentStorage<Menu>>());
        fixture.Register(() => settings.AsOption());
#endif
        
        fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
        return fixture;
    }
}
