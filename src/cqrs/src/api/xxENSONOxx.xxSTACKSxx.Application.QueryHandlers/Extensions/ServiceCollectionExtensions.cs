using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Queries;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
#if CosmosDb || DynamoDb
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
#endif

namespace xxENSONOxx.xxSTACKSxx.Application.QueryHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetMenuById, Menu>, GetMenuByIdQueryHandler>();
#if CosmosDb || DynamoDb
        services.AddTransient<IQueryHandler<SearchMenu, SearchMenuResult>, SearchMenuQueryHandler>();
#endif
        return services;
    }
}
