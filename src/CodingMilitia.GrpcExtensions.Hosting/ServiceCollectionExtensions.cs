using System;
using System.Collections.Generic;
using CodingMilitia.GrpcExtensions.Hosting.Internal;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection, Server server)
        {
            serviceCollection.AddSingleton(server);
            serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService>();
            return serviceCollection;
        }

        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection, Func<IServiceProvider, Server> serverFactory)
        {
            serviceCollection.AddSingleton(serverFactory);
            serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService>();
            return serviceCollection;
        }

        public static IServiceCollection AddGrpcServer<TService>(
            this IServiceCollection serviceCollection,
            Func<TService, ServerServiceDefinition> serviceBinder,
            IEnumerable<ServerPort> ports,
            IEnumerable<ChannelOption> channelOptions = null
        )
            where TService : class
        {
            serviceCollection.AddSingleton<TService>();
            serviceCollection.AddSingleton(appServices =>
            {
                var server = channelOptions != null ? new Server(channelOptions) : new Server();
                server.AddPorts(ports);
                server.AddServices(serviceBinder(appServices.GetRequiredService<TService>()));
                return server;
            });
            serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService>();
            return serviceCollection;
        }

        private static void AddPorts(this Server server, IEnumerable<ServerPort> ports)
        {
            foreach (var port in ports)
            {
                server.Ports.Add(port);
            }
        }

        private static void AddServices(this Server server, params ServerServiceDefinition[] services)
        {
            server.AddServices((IEnumerable<ServerServiceDefinition>)services);
        }

        private static void AddServices(this Server server, IEnumerable<ServerServiceDefinition> services)
        {
            foreach (var service in services)
            {
                server.Services.Add(service);
            }
        }
    }
}