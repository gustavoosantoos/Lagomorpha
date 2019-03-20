using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Lagomorpha
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLagomorpha(this IServiceCollection services, Assembly rootAssembly = null)
        {
            var callerAssembly = rootAssembly ?? Assembly.GetCallingAssembly();
            var methods = callerAssembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(QueueHandlerAttribute), false).Length > 0)
                .ToArray();

            foreach (var methodInfo in methods)
            {
                var attribute = methodInfo.GetCustomAttribute<QueueHandlerAttribute>();
                RabbitQueueEngine.Instance.HandlersDefinitions.Add(attribute.QueueName, methodInfo);
            }
        }
    }
}
