using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class GrpcServerBuilder : IGrpcServerBuilder
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IEnumerable<ServerPort> _ports;
        private readonly IEnumerable<ChannelOption> _channelOptions;
        private readonly List<ServiceRegistrationInfo> _registrationInfo;
        private bool _built;

        public GrpcServerBuilder(IServiceCollection serviceCollection, IEnumerable<ServerPort> ports, IEnumerable<ChannelOption> channelOptions)
        {
            if (ports == null)
            {
                throw new ArgumentNullException(nameof(ports));
            }

            if (!ports.Any())
            {
                throw new ArgumentException(message: "At least one port must be specified", paramName: nameof(ports));
            }

            _serviceCollection = serviceCollection;
            _ports = ports;
            _channelOptions = channelOptions ?? Array.Empty<ChannelOption>();
            _registrationInfo = new List<ServiceRegistrationInfo>();
            _built = false;

        }

        public IGrpcServerBuilder AddService<TService>() where TService : class
        {
            ThrowIfServerAlreadyBuilt();

            var serviceType = typeof(TService);
            if (_serviceCollection.Any(s => s.ServiceType.Equals(serviceType)) || _registrationInfo.Any(s => s.ServiceType.Equals(serviceType)))
            {
                throw new InvalidOperationException($"{typeof(TService).Name} is already registered in the container.");
            }

            _serviceCollection.AddSingleton<TService>();
            var serviceBinder = ServerBuildHelpers.GetServiceBinder<TService>();
            //Storing a lambda to use later, because this avoids reflection tricks later when we don't have access to the TService type so easily to invoke the binder.
            //Not invoking it immediately to keep it lazy.
            _registrationInfo.Add(new ServiceRegistrationInfo(serviceType, appServices => serviceBinder(appServices.GetRequiredService<TService>())));

            return this;
        }

        public void AddServerToServiceCollection()
        {
            ThrowIfServerAlreadyBuilt();

            if (_registrationInfo.Count == 0)
            {
                throw new InvalidOperationException("Must add at least one service for a server to be created.");
            }

            _serviceCollection.AddSingleton(appServices =>
           {
               var server = _channelOptions != null && _channelOptions.Count() > 0 ? new Server(_channelOptions) : new Server();
               server.AddPorts(_ports);
               foreach (var serviceDefinition in _registrationInfo)
               {
                   server.AddServices(serviceDefinition.ServiceDefinitionProvider(appServices));
               }
               return server;
           });

            _built = true;
        }

        private void ThrowIfServerAlreadyBuilt()
        {
            if (_built)
            {
                throw new InvalidOperationException("Server already build.");
            }
        }

        private class ServiceRegistrationInfo
        {
            public Type ServiceType { get; }
            public Func<IServiceProvider, ServerServiceDefinition> ServiceDefinitionProvider { get; }

            public ServiceRegistrationInfo(Type serviceType, Func<IServiceProvider, ServerServiceDefinition> serviceDefinitionProvider)
            {
                ServiceType = serviceType;
                ServiceDefinitionProvider = serviceDefinitionProvider;
            }
        }
    }
}
