using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amido.Stacks.DependencyInjection
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns a dictionary with the Implemented Interface as key and the type that implements it
        /// </summary>
        /// <param name="assembly">Assembly containing the implementations of the interface</param>
        /// <param name="openGenericType">The interface being implemented</param>
        /// <returns></returns>
        public static List<(Type interfaceVariation, Type implementation)> GetImplementationsOf(this Assembly assembly, Type openGenericType)
        {
            List<(Type, Type)> implementations = new List<(Type, Type)>();
            foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface))
            {
                foreach (var implementedInterface in type.GetInterfaces()
                                                         .Where(x => x == openGenericType || 
                                                                      (
                                                                          x.IsGenericType &&
                                                                          x.GetGenericTypeDefinition() == openGenericType
                                                                      )))
                {
                    // Make sure we can resolve both the ICommandHandler and the specific type
                    //services.AddTransient(implementedInterface, type);
                    //services.AddTransient(type);
                    implementations.Add((implementedInterface, type));
                }
            }

            return implementations;
        }
    }
}
