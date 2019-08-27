using Lagomorpha.Providers;
using System;
using System.Reflection;

namespace Lagomorpha.Abstractions
{
    public interface ILagomorphaConfigurationBuilder
    {
        ILagomorphaConfigurationBuilder UseRabbitMQ();
        ILagomorphaConfigurationBuilder WithUri(Uri uri);
        ILagomorphaConfigurationBuilder WithHost(string host);
        ILagomorphaConfigurationBuilder WithPort(int port);
        ILagomorphaConfigurationBuilder WithQueuePrefix(string prefix);
        ILagomorphaConfigurationBuilder WithUsername(string username);
        ILagomorphaConfigurationBuilder WithPassword(string password);
        ILagomorphaConfigurationBuilder WithDefaultAssembly(Assembly assembly);

        ILagomorphaConfiguration Build();
    }
}
