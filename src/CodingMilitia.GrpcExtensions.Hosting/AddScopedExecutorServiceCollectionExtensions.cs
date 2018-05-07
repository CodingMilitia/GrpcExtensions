using CodingMilitia.GrpcExtensions.Hosting;
using CodingMilitia.GrpcExtensions.Hosting.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddScopedExecutorServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedExecutor(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new System.ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<IScopedExecutor, ScopedExecutor>();
            return serviceCollection;
        }
    }
}