using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection GetImplementationsOf(this Assembly assembly, Type openGenericType)
        //{
        //    foreach (var type in assemblies.GetAllTypes().Where(x => !x.IsAbstract && !x.IsInterface))
        //    {
        //        foreach (var implementedInterface in type.GetInterfaces()
        //                                                 .Where(x =>
        //                                                    x.IsGenericType
        //                                                    && x.GetGenericTypeDefinition() == openGenericType))
        //        {
        //            // Make sure we can resolve both the ICommandHandler and the specific type
        //            services.AddTransient(implementedInterface, type);
        //            services.AddTransient(type);
        //        }
        //    }
        //}

        //private static IEnumerable<Type> GetAllTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null) =>
        //    assemblies.SelectMany(x => x.GetTypes().Where(t => predicate == null || predicate(t)));

        //private static IServiceCollection AddTransient(this IServiceCollection services, IEnumerable<Type> types) =>
        //    services.ForEach(types, t => services.AddTransient(t));

        //private static IServiceCollection AddScoped(this IServiceCollection services, IEnumerable<Type> types) =>
        //    services.ForEach(types, t => services.AddScoped(t));

        //private static IServiceCollection AddSingleton(this IServiceCollection services, IEnumerable<Type> types) =>
        //    services.ForEach(types, t => services.AddSingleton(t));

        //private static IServiceCollection AddTransient(this IServiceCollection services, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null) =>
        //    services.AddTransient(assemblies.GetAllTypes(predicate));

        //private static IServiceCollection AddScoped(this IServiceCollection services, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null) =>
        //    services.AddScoped(assemblies.GetAllTypes(predicate));

        //private static IServiceCollection AddSingleton(this IServiceCollection services, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null) =>
        //    services.AddSingleton(assemblies.GetAllTypes(predicate));

        //private static IServiceCollection ForEach(this IServiceCollection services, IEnumerable<Type> types, Action<Type> action)
        //{
        //    foreach (var t in types)
        //    {
        //        action(t);
        //    }
        //    return services;
        //}
    }
}
