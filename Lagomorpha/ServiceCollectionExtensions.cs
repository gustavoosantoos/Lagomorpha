using Lagomorpha.Abstractions;
using Lagomorpha.Providers;
using Lagomorpha.Providers.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lagomorpha
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLagomorpha(this IServiceCollection services, Action<ILagomorphaConfigurationBuilder> builderAction)
        {
            var builder = new LagomorphaConfigurationBuilder();
            builderAction(builder);
            var configuration = builder.Build();

            services.AddSingleton(configuration);

            switch (configuration.Provider)
            {
                case EProviders.RabbitMQ:
                default:
                    services.AddRabbitMQProviderServices();
                    break;
            }
        }
    }
}
