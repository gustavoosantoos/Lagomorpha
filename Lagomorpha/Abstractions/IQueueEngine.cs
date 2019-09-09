using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Lagomorpha.Abstractions
{
    public interface IQueueEngine
    {
        Dictionary<string, MethodInfo[]> HandlersDefinitions { get; }

        Task DispatchHandlerCall(MethodInfo handlerToDispatch, object handlerCaller, string arg);
    }
}