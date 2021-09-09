using GTerm.NET.AppPreferences;
using GTerm.NET.Contracts;
using GTerm.NET.Terminals;

using IGE.ApplicationPreferences;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
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

            IEnumerable<Type> types = GetBaseTerminalImplementations(assembly);

            foreach (Type terminalType in types)
            {
                services.AddTransient(typeof(BaseTerminal), terminalType);
                services.AddTransient(terminalType, terminalType);
            }

            services.AddSingleton(Preferences<TerminalPreferences>.Create("TerminalPreferences"));

            services.AddSingleton<Func<BaseTerminal>>(TerminalFactory);

            return services;
        }

        private static IEnumerable<Type> GetBaseTerminalImplementations(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(BaseTerminal)))
                .Where(myType => !myType.IsAbstract)
                .ToList();
        }

        private static Func<IServiceProvider, Func<BaseTerminal>> TerminalFactory =>
            services =>
            {
                return () =>
                    {

                        var terminalPreferences = services.GetService<Preferences<TerminalPreferences>>().Value;

                        var types = GetBaseTerminalImplementations(Assembly.GetExecutingAssembly());

                        foreach (var type in types)
                        {

                            var fields = type.GetFields()
                                .Where(fi => fi.IsLiteral && !fi.IsInitOnly);
                            
                            if (!fields.Any())
                                continue;

                            var val = fields.FirstOrDefault().GetRawConstantValue() as string;

                            if (val == terminalPreferences.SelectedTerminal)
                            {
                                var term = services.GetService(type) as BaseTerminal;

                                return term;
                            }
                        }

                        return services.GetService<DefaultTerminal>();
                    };
            };
    }
}
