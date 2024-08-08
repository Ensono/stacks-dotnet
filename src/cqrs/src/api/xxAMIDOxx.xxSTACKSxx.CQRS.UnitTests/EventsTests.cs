using System;
using System.Linq;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Amido.Stacks.DependencyInjection;
using AutoFixture;
using AutoFixture.Kernel;
using NSubstitute;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Common.Events;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.UnitTests;

/// <summary>
/// Series of tests for Events to ensure consistency and conventions
/// </summary>
[Trait("TestType", "UnitTests")]
public class EventsTests
{
    [Fact]
    public void EventsShouldHaveUniqueIds()
    {
        var assembly = typeof(MenuCreatedEvent).Assembly;
        var definitions = assembly.GetImplementationsOf(typeof(IApplicationEvent));

        var duplicateCodes = definitions.Select(d => new
        {
            EventCode = GetEventCode(d.implementation),
            d.implementation.Name
        })
            .GroupBy(i => i.EventCode)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

        int duplicates = duplicateCodes.Length;
        duplicates.ShouldBe(0, $"Assembly {assembly.FullName} has duplicate events for event code:" + string.Join(", ", duplicateCodes));
    }

    [Fact]

    public void EventNameShouldMatchOperationName()
    {
        var definitions = typeof(MenuCreatedEvent).Assembly.GetImplementationsOf(typeof(IApplicationEvent));
        foreach (var definition in definitions)
        {
            var eventCode = GetEventCode(definition.implementation);
            var eventName = GetEventName(eventCode);

            // If the user intend to use the type as part of the name for convention,
            // the convention should be nameApplicationEvent not nameEvent
            // Event is generic and can mislead with DomainEvents
            definition.implementation.Name.ShouldBeOneOf(eventName, $"{eventName}Event");
        }
    }

    [Fact]

    public void EventCodeShouldHaveOneImplementation()
    {
        var definitions = typeof(MenuCreatedEvent).Assembly.GetImplementationsOf(typeof(IApplicationEvent));
        foreach (EventCode code in Enum.GetValues(typeof(EventCode)))
        {
            var implementation = definitions.Select(d => d.implementation).SingleOrDefault(o => GetEventCode(o) == (int)code);
            implementation.ShouldNotBeNull($"The event '{(int)code}-{code.ToString()}' does not have an implementation");
        }
    }

    private int GetEventCode(Type eventType)
    {
        var fixture = new Fixture();
        fixture.Register<IOperationContext>(() => Substitute.For<IOperationContext>());
        var cmd = new SpecimenContext(fixture).Resolve(eventType);
        return ((IApplicationEvent)cmd).EventCode;
    }

    private string GetEventName(int eventCode)
    {
        return ((EventCode)eventCode).ToString();
    }
}
