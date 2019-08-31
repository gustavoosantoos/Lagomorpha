using System.Collections.Generic;
using System.Reflection;

namespace Lagomorpha.Abstractions
{
    public interface IQueueEngine
    {
        Dictionary<string, MethodInfo[]> HandlersDefinitions { get; }

        void DispatchHandlerCall(MethodInfo handlerToDispatch, object handlerCaller, string arg);
    }
}