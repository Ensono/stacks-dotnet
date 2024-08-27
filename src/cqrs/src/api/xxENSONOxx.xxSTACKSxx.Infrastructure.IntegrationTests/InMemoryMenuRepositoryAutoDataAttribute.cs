using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Fakes;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class InMemoryMenuRepositoryAutoDataAttribute() : AutoDataAttribute(Customizations)
{
    public static IFixture Customizations()
    {
        IFixture fixture = new Fixture();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<InMemoryMenuRepository>(loggerFactory));
        fixture.Register<ILogger<InMemoryMenuRepository>>(() => new Logger<InMemoryMenuRepository>(loggerFactory));

        return fixture;
    }
}
