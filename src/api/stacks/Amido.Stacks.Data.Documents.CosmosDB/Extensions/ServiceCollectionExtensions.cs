using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.Data.Documents.CosmosDB.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the CosmosDB singleton components for IDocumentStorage<,> and IDocumentSearch<>
        /// This will create one singleton instance per Container(Where the container map to TEntity name)
        /// </summary>
        public static IServiceCollection AddCosmosDB(this IServiceCollection services)
        {
            // CosmosDB components are thread safe and should be singleton to avoid opening new 
            // connections on every request, similar to HttpCliient

            services.AddSingleton(typeof(IDocumentStorage<>), typeof(CosmosDbDocumentStorage<>));
            services.AddSingleton(typeof(IDocumentSearch<>), typeof(CosmosDbDocumentStorage<>));
            services.AddSingleton(typeof(CosmosDbDocumentStorage<>));
            return services;
        }
    }
}
