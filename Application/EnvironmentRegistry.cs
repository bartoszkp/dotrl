using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Core;
using Environments;
using Environments.Infrastructure;
using Presenters;

namespace Application
{
    public static class EnvironmentRegistry
    {
        static EnvironmentRegistry()
        {
            Initialize();
        }

        public static IEnumerable<Type> GetEnvironments()
        {
            Contract.Ensures(Contract.Result<Func<object, Presenter>>() != null);

            return environments.SelectMany(kvp => kvp.Value);
        }

        public static IEnumerable<Type> GetEnvironments(ComponentType componentType)
        {
            Contract.Requires(componentType != null);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            return environments.ContainsKey(componentType)
                ? environments[componentType]
                : Enumerable.Empty<Type>();
        }

        public static Func<Component, Presenter> GetPresenterFactoryForEnvironment(Type environmentType)
        {
            Contract.Requires(environmentType != null && environmentType.IsEnvironment());
            Contract.Ensures(Contract.Result<Func<object, Presenter>>() != null);

            return presenterFactories[environmentType];
        }
        
        private static void Initialize()
        {
            environments = new Dictionary<ComponentType, List<Type>>();
            presenterFactories = new Dictionary<Type, Func<Component, Presenter>>();

            Type[] allEnvironmentTypes = Assembly
                .GetAssembly(typeof(Environment<,>))
                .GetTypes()
                .Where(t => t.IsEnvironment())
                .ToArray();
            Type[] allPresenterTypes = Assembly
                .GetAssembly(typeof(Presenter))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Presenter)))
                .ToArray();

            Func<ConstructorInfo, bool> constructorTakingOnlyEnvironmentParameterPredicate
             = c => c.GetParameters().Length == 1
                 && c.GetParameters().First().ParameterType.BaseType != null
                 && c.GetParameters().First().ParameterType.BaseType.IsGenericType
                 && c.GetParameters().First().ParameterType.BaseType.GetGenericTypeDefinition().Equals(typeof(Environment<,>));

            foreach (Type environmentType in allEnvironmentTypes)
            {
                Func<object, Presenter> presenterFactory = environment
                    =>
                    {
                        Type basicPresenterType = typeof(BasicPresenter<,>);
                        Type[] genericArguments = environment.GetType().BaseType.GetGenericArguments();

                        return basicPresenterType.MakeGenericType(genericArguments).GetConstructor(new[] { environment.GetType() }).Invoke(new[] { environment })
                            as Presenter;
                    };

                ConstructorInfo presenterConstructor = allPresenterTypes
                    .SelectMany(p => p.GetConstructors())
                    .Where(c => c.GetParameters().Length == 1)
                    .Where(constructorTakingOnlyEnvironmentParameterPredicate)
                    .SingleOrDefault(c => c.GetParameters()[0].ParameterType.Equals(environmentType));

                if (presenterConstructor != null)
                {
                    presenterFactory = environment
                        =>
                        {
                            return presenterConstructor.Invoke(new[] { environment })
                                as Presenter;
                        };
                }

                AddEnvironment(environmentType, presenterFactory);
            }
        }

        private static void AddEnvironment(Type environment, Func<object, Presenter> presenterFactory)
        {
            Type[] types = environment.BaseType.GetGenericArguments();
            ComponentType componentType = new ComponentType(types[0], types[1]);
            if (!environments.ContainsKey(componentType))
            {
                environments.Add(componentType, new List<Type>());
            }

            environments[componentType].Add(environment);
            presenterFactories.Add(environment, presenterFactory);
        }

        private static Dictionary<ComponentType, List<Type>> environments;
        private static Dictionary<Type, Func<Component, Presenter>> presenterFactories;
    }
}
