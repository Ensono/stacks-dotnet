using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Data.Documents.CosmosDB;
using Amido.Stacks.Data.Documents.CosmosDB.Extensions;
using Amido.Stacks.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.HealthChecks;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure
{
    public static class DependencyRegistration
    {
        static readonly ILogger log = Log.Logger;

        /// <summary>
        /// Register static services that does not change between environment or contexts(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureStaticDependencies(IServiceCollection services)
        {
            AddCommandHandlers(services);
            AddQueryHandlers(services);
        }

        /// <summary>
        /// Register dynamic services that changes between environments or context(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureProductionDependencies(WebHostBuilderContext context, IServiceCollection services)
        {
            services.Configure<CosmosDbConfiguration>(context.Configuration.GetSection("CosmosDB"));

            services.AddSecrets();
            services.AddCosmosDB();

            //TODO: Evaluate if event publishers should be generic, probably not, EventHandler are generic tough
            AddEventPublishers(services);

            if (Environment.GetEnvironmentVariable("USE_MEMORY_STORAGE") == null)
                services.AddTransient<IMenuRepository, MenuRepository>();
            else
                services.AddTransient<IMenuRepository, InMemoryMenuRepository>();

            var healthChecks = services.AddHealthChecks();
            healthChecks.AddCheck<CosmosDbDocumentStorage<Menu>>("CosmosDB");
            healthChecks.AddCheck<CustomHealthCheck>("Sample");//This is a sample health check, remove if not needed, more info: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
        }

        private static void AddCommandHandlers(IServiceCollection services)
        {
            log.Information("Loading implementations of {interface}", typeof(ICommandHandler<,>).Name);
            var definitions = typeof(CreateMenuCommandHandler).Assembly.GetImplementationsOf(typeof(ICommandHandler<,>));
            foreach (var definition in definitions)
            {
                log.Information("Registering '{implementation}' as implementation of '{interface}'", definition.implementation.FullName, definition.interfaceVariation.FullName);
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        private static void AddQueryHandlers(IServiceCollection services)
        {
            log.Information("Loading implementations of {interface}", typeof(IQueryHandler<,>).Name);
            var definitions = typeof(GetMenuByIdQueryHandler).Assembly.GetImplementationsOf(typeof(IQueryHandler<,>));
            foreach (var definition in definitions)
            {
                log.Information("Registering '{implementation}' as implementation of '{interface}'", definition.implementation.FullName, definition.interfaceVariation.FullName);
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        private static void AddEventPublishers(IServiceCollection services)
        {
            log.Information("Loading implementations of {interface}", typeof(IApplicationEventPublisher).Name);
            var definitions = typeof(DummyEventPublisher).Assembly.GetImplementationsOf(typeof(IApplicationEventPublisher));
            foreach (var definition in definitions)
            {
                log.Information("Registering '{implementation}' as implementation of '{interface}'", definition.implementation.FullName, definition.interfaceVariation.FullName);
                //TODO: maybe this should be singleton
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }
    }
}
