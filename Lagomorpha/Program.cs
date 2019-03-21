using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Lagomorpha
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging(l => l.AddConsole())
                .ConfigureAppConfiguration(c => c.AddEnvironmentVariables())
                .ConfigureServices(s =>
                {
                    s.AddLagomorpha();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
