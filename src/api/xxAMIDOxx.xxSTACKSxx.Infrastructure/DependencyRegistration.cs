using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Events;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure
{
    public static class DependencyRegistration
    {
        /// <summary>
        /// Register static services that does not change between environment or contexts(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureStaticServices(IServiceCollection services)
        {
            AddCommandHandlers(services);
            AddQueryHandlers(services);
        }

        /// <summary>
        /// Register dynamic services that changes between environments or context(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddTransient<IMenuRepository, MenuRepository>();
            //TODO: Evaluate if event publishers should be generic
            AddEventPublishers(services);
        }

        public static void AddCommandHandlers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(ICommandHandler<>).Name}");
            var definitions = typeof(CreateMenuCommandHandler).Assembly.GetImplementationsOf(typeof(ICommandHandler<>));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        public static void AddQueryHandlers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(IQueryHandler<,>).Name}");
            var definitions = typeof(GetMenuByIdQueryHandler).Assembly.GetImplementationsOf(typeof(IQueryHandler<,>));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        private static void AddEventPublishers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(IApplicationEventPublisher).Name}");
            var definitions = typeof(DummyEventPublisher).Assembly.GetImplementationsOf(typeof(IApplicationEventPublisher));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

    }
}
