using Lagomorpha.Providers;
using System;
using System.Reflection;

namespace Lagomorpha.Abstractions
{
    public interface ILagomorphaConfiguration
    {
        EProviders Provider { get; set; }
        Uri Uri { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string QueuePrefix { get; set; }
        Assembly Assembly { get; set; }
    }
}
