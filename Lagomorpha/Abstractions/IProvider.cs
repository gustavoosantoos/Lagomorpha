using Microsoft.Extensions.DependencyInjection;

namespace Lagomorpha.Abstractions
{
    public interface IProvider
    {
        void RegisterServices(IServiceCollection services);
    }
}
