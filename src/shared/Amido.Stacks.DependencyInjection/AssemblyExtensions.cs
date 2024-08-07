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
        public static List<(Type interfaceVariation, Type implementation)> GetImplementationsOf(this Assembly assembly, params Type[] openGenericTypes)
        {
            List<(Type, Type)> implementations = new List<(Type, Type)>();
            foreach (var openGenericType in openGenericTypes)
            {
                foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface))
                {
                    foreach (var implementedInterface in type.GetInterfaces()
                                                             .Where(x => x == openGenericType ||
                                                                          (
                                                                              x.IsGenericType &&
                                                                              x.GetGenericTypeDefinition() == openGenericType
                                                                          )))
                    {
                        implementations.Add((implementedInterface, type));
                    }
                }
            }

            return implementations;
        }
    }
}
