using System;

namespace Lagomorpha
{
    public class FooBarHandler : RabbitQueueWorker
    {
        [QueueHandler("ProductQueue")]
        public void Handle(Product product)
        {

        }
    }

    public class Product
    {
    }

    internal class QueueHandlerAttribute : Attribute
    {
        public string QueueName { get; }
        public InputFormat Format { get; }

        public QueueHandlerAttribute(string queueName, InputFormat format = InputFormat.Json)
        {
            Format = format;
            QueueName = queueName;
        }
    }

    public enum InputFormat
    {
        Json,
        Xml
    }
}
