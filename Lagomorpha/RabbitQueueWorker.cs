using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Lagomorpha
{
    public class RabbitQueueWorker : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly ConnectionFactory _connectionFactory;


        public RabbitQueueWorker(IServiceProvider provider)
        {
            _provider = provider;
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var handlerToDispatch = RabbitQueueEngine.Instance.HandlersDefinitions[e.RoutingKey];

                using (var scope = _provider.CreateScope())
                {
                    var handlerClass = scope.ServiceProvider.GetService(handlerToDispatch.DeclaringType);
                    var parameterType = handlerToDispatch.GetParameters()[0].ParameterType;
                    var convertedParameter = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body), parameterType);

                    handlerToDispatch.Invoke(handlerClass, new[] { convertedParameter });
                    channel.BasicAck(e.DeliveryTag, false);
                }
            };

            foreach (var queue in RabbitQueueEngine.Instance.HandlersDefinitions.Keys)
            {
                channel.QueueDeclare(queue, true, false, false);
                channel.BasicConsume(queue, false, consumer);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}