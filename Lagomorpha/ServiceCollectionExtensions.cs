using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Lagomorpha
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLagomorpha(this IServiceCollection services, Type assemblyType)
        {
            services.AddSingleton(s => new RabbitQueueEngine(assemblyType.Assembly));
            services.AddHostedService<RabbitQueueWorker>();
        }
    }
}
