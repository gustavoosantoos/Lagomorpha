using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Lagomorpha
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLagomorpha(this IServiceCollection services, Assembly rootAssembly = null)
        {
            services.AddHostedService<RabbitQueueWorker>();
        }
    }
}
