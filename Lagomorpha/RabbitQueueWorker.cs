using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Lagomorpha
{
    public class RabbitQueueWorker : IHostedService
    {


        public Task StartAsync(CancellationToken cancellationToken)
        {

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }   
}