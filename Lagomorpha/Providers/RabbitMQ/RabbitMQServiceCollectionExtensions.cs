using Lagomorpha.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Lagomorpha.Providers.RabbitMQ
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static void AddRabbitMQProvider(this IServiceCollection services)
        {
            services.AddScoped<IQueueEngine, RabbitQueueEngine>();
            services.AddHostedService<RabbitQueueWorker>();
        }
    }
}
