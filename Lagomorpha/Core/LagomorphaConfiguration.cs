using Lagomorpha.Abstractions;
using Lagomorpha.Providers;
using System;
using System.Reflection;

namespace Lagomorpha
{
    public class LagomorphaConfiguration : ILagomorphaConfiguration
    {
        public EProviders Provider { get; set; }
        public Uri Uri { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueuePrefix { get; set; }
        public Assembly Assembly { get; set; }
    }
}
