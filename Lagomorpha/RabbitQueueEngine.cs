using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lagomorpha
{
    public class RabbitQueueEngine
    {
        private static readonly Lazy<RabbitQueueEngine> _instance =
            new Lazy<RabbitQueueEngine>(() => new RabbitQueueEngine());

        public static RabbitQueueEngine Instance => _instance.Value;

        private RabbitQueueEngine()
        {
            HandlersDefinitions = new Dictionary<string, MethodInfo>();
            LoadDefinitions(GetMethodHandlers());
        }
        
        public Dictionary<string, MethodInfo> HandlersDefinitions { get; private set; }
        
        private static MethodInfo[] GetMethodHandlers()
        {
            return Assembly
                .GetCallingAssembly()
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
