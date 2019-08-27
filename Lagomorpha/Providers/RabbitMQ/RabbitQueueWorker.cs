﻿using Lagomorpha.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
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
                _connectionFactory.UserName = configuration.Username ?? ConnectionFactory.DefaultUser;
                _connectionFactory.Password = configuration.Password ?? ConnectionFactory.DefaultPass;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
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
    }
}