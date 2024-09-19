using System;
using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.Application.CommandHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<CreateCategory, Guid>, CreateCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteCategory, bool>, DeleteCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateCategory, bool>, UpdateCategoryCommandHandler>();
        services.AddTransient<ICommandHandler<CreateMenu, Guid>, CreateMenuCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteMenu, bool>, DeleteMenuCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateMenu, bool>, UpdateMenuCommandHandler>();
        services.AddTransient<ICommandHandler<CreateMenuItem, Guid>, CreateMenuItemCommandHandler>();
        services.AddTransient<ICommandHandler<DeleteMenuItem, bool>, DeleteMenuItemCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateMenuItem, bool>, UpdateMenuItemCommandHandler>();
        return services;
    }
}
