using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lagomorpha
{
    public class RabbitQueueWorker : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;
        private readonly RabbitQueueEngine _engine;
        private readonly ConnectionFactory _connectionFactory;


        public RabbitQueueWorker(IServiceProvider provider, IConfiguration configuration, RabbitQueueEngine engine)
        {
            _engine = engine;
            _provider = provider;
            _configuration = configuration;
            _connectionFactory = new ConnectionFactory();

            var uri = _configuration.GetSection("RabbitMQ:Uri").Value;

            if (uri != null)
            {
                _connectionFactory.Uri = new Uri(uri);
            }
            else
            {
                _connectionFactory.HostName = _configuration.GetSection("RabbitMQ:HostName").Value ?? "localhost";
                _connectionFactory.UserName = _configuration.GetSection("RabbitMQ:UserName").Value ?? ConnectionFactory.DefaultUser;
                _connectionFactory.Password = _configuration.GetSection("RabbitMQ:Password").Value ?? ConnectionFactory.DefaultPass;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                using (var scope = _provider.CreateScope())
                {
                    var handlerToDispatch = _engine.HandlersDefinitions[e.RoutingKey];
                    var handlerClass = scope.ServiceProvider.GetService(handlerToDispatch.DeclaringType);

                    _engine.DispatchHandlerCall(e.RoutingKey, handlerClass, Encoding.UTF8.GetString(e.Body));
                    channel.BasicAck(e.DeliveryTag, false);
                }
            };

            foreach (var queue in _engine.HandlersDefinitions.Keys)
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