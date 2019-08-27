using Lagomorpha.Abstractions;
using Lagomorpha.Providers;
using System;
using System.Reflection;

namespace Lagomorpha
{
    public class LagomorphaConfigurationBuilder : ILagomorphaConfigurationBuilder
    {
        private readonly ILagomorphaConfiguration _configuration;

        public LagomorphaConfigurationBuilder()
        {
            _configuration = new LagomorphaConfiguration();
        }

        public ILagomorphaConfigurationBuilder WithHost(string host)
        {
            _configuration.Host = host;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithPort(int port)
        {
            _configuration.Port = port;
            return this;
        }

        public ILagomorphaConfigurationBuilder UseRabbitMQ()
        {
            _configuration.Provider = EProviders.RabbitMQ;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithQueuePrefix(string prefix)
        {
            _configuration.QueuePrefix = prefix;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithUri(Uri uri)
        {
            _configuration.Uri = uri;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithUsername(string username)
        {
            _configuration.Username = username;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithPassword(string password)
        {
            _configuration.Password = password;
            return this;
        }

        public ILagomorphaConfigurationBuilder WithDefaultAssembly(Assembly assembly)
        {
            _configuration.Assembly = assembly;
            return this;
        }

        public ILagomorphaConfiguration Build()
        {
            return new LagomorphaConfiguration
            {
                Uri = _configuration.Uri,
                Assembly = _configuration.Assembly,
                Username = _configuration.Username,
                Password = _configuration.Password,
                Host = _configuration.Host,
                Port = _configuration.Port,
                Provider = _configuration.Provider,
                QueuePrefix = _configuration.QueuePrefix
            };
        }
    }
}
