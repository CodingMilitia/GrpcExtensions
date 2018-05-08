using CodingMilitia.GrpcExtensions.Hosting;
using CodingMilitia.GrpcExtensions.Hosting.Internal;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddGrpcServerServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a <see cref="Server"/> in the dependency injection container to be hosted in a <see cref="IHostedService"/>.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/> to register the server to.</param>
        /// <param name="server">The <see cref="Server"/> to register.</param>
        /// <returns>The <see cref="IServiceCollection"/> to which the server was registerd.</returns>
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection, Server server)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            serviceCollection.AddSingleton(server);
            serviceCollection.AddGrpcBackgroundServiceIfNotAlreadyRegistered();
            return serviceCollection;
        }

        /// <summary>
        /// Registers a <see cref="Server"/> factory in the dependency injection container, for its provider <see cref="Server"/> to be hosted in a <see cref="IHostedService"/>.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/> to register the server to.</param>
        /// <param name="serverFactory">Factory that provides an <see cref="Server"/> instance for hosting.</param>
        /// <returns>The <see cref="IServiceCollection"/> to which the server factory was registerd.</returns>
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection, Func<IServiceProvider, Server> serverFactory)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (serverFactory == null)
            {
                throw new ArgumentNullException(nameof(serverFactory));
            }

            serviceCollection.AddSingleton(serverFactory);
            serviceCollection.AddGrpcBackgroundServiceIfNotAlreadyRegistered();
            return serviceCollection;
        }

        /// <summary>
        /// Registers a service in the dependency injection container to be included in a <see cref="Server"/> and be hosted in a <see cref="IHostedService"/>.
        /// </summary>
        /// <typeparam name="TService">the type of the service to register.</typeparam>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/> to register the service to.</param>
        /// <param name="ports">The ports the service should listen to.</param>
        /// <param name="channelOptions">The options for the service's channels.</param>
        /// <returns>The <see cref="IServiceCollection"/> to which the service was registerd.</returns>
        public static IServiceCollection AddGrpcServer<TService>(
            this IServiceCollection serviceCollection,
            IEnumerable<ServerPort> ports,
            IEnumerable<ChannelOption> channelOptions = null
        )
            where TService : class
        {
            return serviceCollection.AddGrpcServer(
                 serverConfigurator: configurator => configurator.AddService<TService>(),
                 ports: ports,
                 channelOptions: channelOptions
             );
        }

        /// <summary>
        /// Registers one or more services in the dependency injection container to be included in a <see cref="Server"/> and be hosted in a <see cref="IHostedService"/>.
        /// </summary>
        /// <param name="serviceCollection">The target <see cref="IServiceCollection"/> to register the services to.</param>
        /// <param name="serverConfigurator">An action that configures a <see cref="IGrpcServerBuilder"/> with the desired services.</param>
        /// <param name="ports">The ports the services should listen to.</param>
        /// <param name="channelOptions">The options for the services' channels.</param>
        /// <returns>The <see cref="IServiceCollection"/> to which the services were registerd.</returns>
        public static IServiceCollection AddGrpcServer(
            this IServiceCollection serviceCollection,
            Action<IGrpcServerBuilder> serverConfigurator,
            IEnumerable<ServerPort> ports,
            IEnumerable<ChannelOption> channelOptions = null
        )
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (serverConfigurator == null)
            {
                throw new ArgumentNullException(nameof(serverConfigurator));
            }

            if (ports == null)
            {
                throw new ArgumentNullException(nameof(ports));
            }

            if (!ports.Any())
            {
                throw new ArgumentException(message: "At least one port must be specified", paramName: nameof(ports));
            }

            var builder = new GrpcServerBuilder(serviceCollection, ports, channelOptions);
            serverConfigurator(builder);
            builder.AddServerToServiceCollection();
            serviceCollection.AddGrpcBackgroundServiceIfNotAlreadyRegistered();
            return serviceCollection;
        }

        /// <summary>
        /// As the AddGrpcServer operations can be called multiple times, we only need to register the <see cref="GrpcBackgroundService"/> one and it'll host all <see cref="Server"/>s.
        /// </summary>
        /// <param name="serviceCollection">The service collection to add the <see cref="GrpcBackgroundService"/> to if required.</param>
        private static void AddGrpcBackgroundServiceIfNotAlreadyRegistered(this IServiceCollection serviceCollection)
        {
            if (!serviceCollection.Any(s => s.ServiceType.Equals(typeof(IHostedService)) && s.ImplementationType.Equals(typeof(GrpcBackgroundService))))
            {
                serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService>();
            }
        }
    }
}