using System.Collections.Generic;
using System.Reflection;

namespace Lagomorpha.Abstractions
{
    public interface IQueueEngine
    {
        Dictionary<string, MethodInfo> HandlersDefinitions { get; }

        void DispatchHandlerCall(string queue, object handlerCaller, string arg);
    }
}