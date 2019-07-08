using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Events;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using xxAMIDOxx.xxSTACKSxx.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;
using xxAMIDOxx.xxSTACKSxx.Integration;
using xxAMIDOxx.xxSTACKSxx.QueryHandlers;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure
{
    public static class DependencyRegistration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMenuRepository, MenuRepository>();

            AddCommandHandlers(services);
            AddQueryHandlers(services);
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
