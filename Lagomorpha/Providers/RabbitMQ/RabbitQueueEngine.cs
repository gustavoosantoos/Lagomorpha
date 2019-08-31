using Lagomorpha.Abstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lagomorpha.Providers.RabbitMQ
{
    public class RabbitQueueEngine : IQueueEngine
    {
        public Dictionary<string, MethodInfo[]> HandlersDefinitions { get; private set; }

        public RabbitQueueEngine(ILagomorphaConfiguration configuration)
        {
            HandlersDefinitions = new Dictionary<string, MethodInfo[]>();
            LoadDefinitions(GetMethodHandlers(configuration.Assembly));
        }

        public void DispatchHandlerCall(MethodInfo handlerToDispatch, object handlerCaller, string arg)
        {
            if (handlerToDispatch.GetParameters().Count() == 0)
                handlerToDispatch.Invoke(handlerCaller, new object[] { });

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
            foreach (var methodInfo in methods.GroupBy(m => m.GetCustomAttribute<QueueHandlerAttribute>().QueueName))
            {
                if (methodInfo.Any(mi => mi.GetParameters().Count() > 1))
                    throw new TargetParameterCountException("Method must have none or one parameter");

                HandlersDefinitions.Add(methodInfo.Key, methodInfo.ToArray());
            }
        }
    }
}
