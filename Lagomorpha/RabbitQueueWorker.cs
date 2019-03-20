using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lagomorpha
{
    public class RabbitQueueWorker : IHostedService
    {
        private readonly IServiceProvider _provider;

        public RabbitQueueWorker(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }   
}