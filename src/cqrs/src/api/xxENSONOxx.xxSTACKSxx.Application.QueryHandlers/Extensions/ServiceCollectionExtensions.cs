using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.SearchMenu;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;

namespace xxENSONOxx.xxSTACKSxx.Application.QueryHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetMenuById, Menu>, GetMenuByIdQueryHandler>();
        services.AddTransient<IQueryHandler<SearchMenu, SearchMenuResult>, SearchMenuQueryHandler>();
        return services;
    }
}
