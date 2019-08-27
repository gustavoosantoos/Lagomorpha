using Lagomorpha.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Lagomorpha.Providers.RabbitMQ
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static void AddRabbitMQProviderServices(this IServiceCollection services)
        {
            services.AddTransient<IQueueEngine, RabbitQueueEngine>();
            services.AddHostedService<RabbitQueueWorker>();
        }
    }
}
