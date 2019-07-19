using System;
using System.Linq;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.DependencyInjection;
using AutoFixture;
using AutoFixture.Kernel;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Common.Events;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.UnitTests
{
    /// <summary>
    /// Series of tests for Events to ensure consistency and conventions
    /// </summary>
    [Trait("TestType", "UnitTests")]
    public class EventsTests
    {
        [Fact]

        public void EventsShouldHaveUniqueIds()
        {
            var assembly = typeof(MenuCreated).Assembly;
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
            var definitions = typeof(MenuCreated).Assembly.GetImplementationsOf(typeof(IApplicationEvent));
            foreach (var definition in definitions)
            {
                var eventCode = GetEventCode(definition.implementation);
                var eventName = GetEventName(eventCode);

                // If the user intend to use the type as part of the name for convention, 
                // the convention should be nameApplicationEvent not nameEvent
                // Event is generic and can mislead with DomainEvents
                definition.implementation.Name.ShouldBeOneOf(eventName, $"{eventName}ApplicationEvent");
            }
        }

        private int GetEventCode(Type eventType)
        {
            var cmd = new SpecimenContext(new Fixture()).Resolve(eventType);
            return ((IApplicationEvent)cmd).EventCode;
        }

        private string GetEventName(int eventCode)
        {
            return ((EventCode)eventCode).ToString();
        }
    }
}
