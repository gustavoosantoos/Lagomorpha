using Lagomorpha.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Lagomorpha.Providers.RabbitMQ
{
    public class RabbitQueueWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly IQueueEngine _engine;
        private readonly ConnectionFactory _connectionFactory;


        public RabbitQueueWorker(ILagomorphaConfiguration configuration, IQueueEngine engine, IServiceProvider provider)
        {
            _engine = engine;
            _provider = provider;
            _connectionFactory = new ConnectionFactory();

            if (configuration.Uri != null)
            {
                _connectionFactory.Uri = configuration.Uri;
            }
            else
            {
                _connectionFactory.HostName = configuration.Host;
                _connectionFactory.Port = configuration.Port;
                _connectionFactory.UserName = configuration.Username ?? ConnectionFactory.DefaultUser;
                _connectionFactory.Password = configuration.Password ?? ConnectionFactory.DefaultPass;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = _connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, e) =>
            {
                using (var scope = _provider.CreateScope())
                {
                    foreach (var handlerToDispatch in _engine.HandlersDefinitions[e.RoutingKey])
                    {
                        var handlerClass = scope.ServiceProvider.GetService(handlerToDispatch.DeclaringType);
                        var response = await _engine.DispatchHandlerCall(handlerToDispatch, handlerClass, Encoding.UTF8.GetString(e.Body.Span));

                        var queueHandlerAttibute = handlerToDispatch.GetCustomAttribute<QueueHandlerAttribute>();
                        if (!string.IsNullOrWhiteSpace(queueHandlerAttibute.ResponseQueue) && response != null)
                        {
                            channel.QueueDeclare(queueHandlerAttibute.ResponseQueue, true, false, false);
                            channel.BasicPublish("", queueHandlerAttibute.ResponseQueue, body: JsonSerializer.SerializeToUtf8Bytes(response));
                        }
                    }

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
    }
}