using CodingMilitia.GrpcExtensions.Hosting;
using CodingMilitia.GrpcExtensions.Hosting.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddScopedExecutorServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedExecutor<TService>(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IScopedExecutor, ScopedExecutor>();
            return serviceCollection;
        }
    }
}