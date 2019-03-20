using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lagomorpha
{
    public class RabbitQueueEngine
    {
        private static readonly Lazy<RabbitQueueEngine> _instance =
            new Lazy<RabbitQueueEngine>(() => new RabbitQueueEngine());

        public static RabbitQueueEngine Instance => _instance.Value;

        private RabbitQueueEngine()
        {
            HandlersDefinitions = new Dictionary<string, MethodInfo>();
        }
        
        public Dictionary<string, MethodInfo> HandlersDefinitions { get; private set; }
           
    }
}
