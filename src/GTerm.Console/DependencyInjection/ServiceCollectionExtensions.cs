using GTerm.NET.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace GTerm.NET.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScreens(this IServiceCollection services, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            var types = assembly
                .GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IScreen)));

            foreach (Type myType in types)
            {
                services.AddScoped(myType);
            }

            return services;
        }

        public static IServiceCollection AddTerminals(this IServiceCollection services, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            var types = assembly
                .GetTypes()
                .Where(myType => myType.GetInterfaces().Contains(typeof(ITerminal)));

            foreach (Type myType in types)
            {
                services.AddScoped(typeof(ITerminal), myType);
                services.AddScoped(myType);
            }

            return services;
        }
    }
}
