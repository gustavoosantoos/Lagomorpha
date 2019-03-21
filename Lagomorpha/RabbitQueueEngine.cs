using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public void DispatchHandlerCall(string queue, object handlerCaller, string arg)
        {
            var handlerToDispatch = HandlersDefinitions[queue];
            var parameterType = handlerToDispatch.GetParameters()[0].ParameterType;
            
            handlerToDispatch.Invoke(handlerCaller, new[] { JsonConvert.DeserializeObject(arg, parameterType) });
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
