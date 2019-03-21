using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lagomorpha
{
    public class RabbitQueueEngine
    {
        public Dictionary<string, MethodInfo> HandlersDefinitions { get; private set; }

        public RabbitQueueEngine(Assembly assembly)
        {
            HandlersDefinitions = new Dictionary<string, MethodInfo>();
            LoadDefinitions(GetMethodHandlers(assembly));
        }
        
        private MethodInfo[] GetMethodHandlers(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(QueueHandlerAttribute), false).Length > 0)
                .ToArray();
        }

        private void LoadDefinitions(MethodInfo[] methods)
        {
            foreach (var methodInfo in methods)
            {
                var attribute = methodInfo.GetCustomAttribute<QueueHandlerAttribute>();
                HandlersDefinitions.Add(attribute.QueueName, methodInfo);
            }
        }
    }
}
