using Lagomorpha.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<object> DispatchHandlerCall(MethodInfo handlerToDispatch, object handlerCaller, string arg)
        {
            if (handlerToDispatch.GetParameters().Count() == 0)
                handlerToDispatch.Invoke(handlerCaller, new object[] { });

            object response;
            var parameterType = handlerToDispatch.GetParameters()[0].ParameterType;
            var methodIsAsync = handlerToDispatch.GetCustomAttribute<AsyncStateMachineAttribute>() != null;

            if (methodIsAsync)
            {
                var task = (Task) handlerToDispatch.Invoke(handlerCaller, new[] { JsonSerializer.Deserialize(arg, parameterType) });
                await task.ConfigureAwait(false);
                var resultProperty = task.GetType().GetProperty("Result");
                response = resultProperty.GetValue(task);
            }
            else
            {
                response = handlerToDispatch.Invoke(handlerCaller, new[] { JsonSerializer.Deserialize(arg, parameterType) });
            }

            return response;
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
            var methodsOrderedAndGrouped = methods
                .OrderBy(m => m.GetCustomAttribute<QueueHandlerAttribute>().Order)
                .GroupBy(m => m.GetCustomAttribute<QueueHandlerAttribute>().QueueName);

            foreach (var methodInfo in methodsOrderedAndGrouped)
            {
                if (methodInfo.Any(mi => mi.GetParameters().Length > 1))
                    throw new TargetParameterCountException("Method must have none or one parameter");

                HandlersDefinitions.Add(methodInfo.Key, methodInfo.ToArray());
            }
        }
    }
}
