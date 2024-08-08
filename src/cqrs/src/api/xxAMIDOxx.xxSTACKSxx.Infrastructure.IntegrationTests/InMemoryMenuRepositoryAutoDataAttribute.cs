using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class InMemoryMenuRepositoryAutoDataAttribute : AutoDataAttribute
{
    public InMemoryMenuRepositoryAutoDataAttribute() : base(Customizations) { }

    public static IFixture Customizations()
    {
        IFixture fixture = new Fixture();

        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<InMemoryMenuRepository>(loggerFactory));
        fixture.Register<ILogger<InMemoryMenuRepository>>(() => new Logger<InMemoryMenuRepository>(loggerFactory));

        return fixture;
    }
}