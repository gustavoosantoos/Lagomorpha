using System;

namespace Lagomorpha
{
    public class FooBarHandler : RabbitQueueWorker
    {
        [QueueHandler("ProductQueue")]
        public void Handle(Product product)
        {

        }

        [QueueHandler("SellQueue")]
        public void Handle(Sell sell)
        {

        }
    }

    public class Sell
    {
    }

    public class Product
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class QueueHandlerAttribute : Attribute
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
