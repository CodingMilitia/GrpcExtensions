using CodingMilitia.GrpcExtensions.Hosting;
using CodingMilitia.GrpcExtensions.Hosting.Internal;
using System;
using System.Linq;

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

            if (serviceCollection.Any(s => s.ServiceType.Equals(typeof(IScopedExecutor)) && s.ImplementationType.Equals(typeof(ScopedExecutor))))
            {
                throw new InvalidOperationException("ScopedExecutor already registered");
            }

            serviceCollection.AddSingleton<IScopedExecutor, ScopedExecutor>();
            return serviceCollection;
        }
    }
}