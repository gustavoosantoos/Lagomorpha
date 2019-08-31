using System;

namespace Lagomorpha
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class QueueHandlerAttribute : Attribute
    {
        public string QueueName { get; }
        public InputFormat Format { get; }
        public int Order { get; set; }

        public QueueHandlerAttribute(string queueName, InputFormat format = InputFormat.Json)
        {
            Format = format;
            QueueName = queueName;
        }
    }
}
