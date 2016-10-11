using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Agents;
using Agents.Infrastructure;
using Core;

namespace Application
{
    public static class AgentRegistry 
    {
        static AgentRegistry()
        {
            Initialize();
        }

        public static IEnumerable<Type> GetAgents(ComponentType componentType)
        {
            Contract.Requires(componentType != null);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            return agents.ContainsKey(componentType)
                ? agents[componentType]
                : Enumerable.Empty<Type>();
        }

        private static void Initialize()
        {
            agents = new Dictionary<ComponentType, List<Type>>();

            Assembly
               .GetAssembly(typeof(Agent<,>))
               .GetTypes()
               .Where(t => t.IsAgent())
               .ToList()
               .ForEach(t => AddAgent(t));
        }

        private static void AddAgent(Type agent)
        {
            Type[] types = agent.BaseType.GetGenericArguments();
            ComponentType componentType = new ComponentType(types[0], types[1]);
            if (!agents.ContainsKey(componentType))
            {
                agents.Add(componentType, new List<Type>());
            }

            agents[componentType].Add(agent);
        }

        private static Dictionary<ComponentType, List<Type>> agents;
    }
}
