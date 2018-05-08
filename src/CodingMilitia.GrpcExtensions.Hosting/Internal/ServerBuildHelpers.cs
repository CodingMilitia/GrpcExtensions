using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal static class ServerBuildHelpers
    {
        public static void AddPorts(this Server server, IEnumerable<ServerPort> ports)
        {
            foreach (var port in ports)
            {
                server.Ports.Add(port);
            }
        }

        public static void AddServices(this Server server, params ServerServiceDefinition[] services)
        {
            server.AddServices((IEnumerable<ServerServiceDefinition>)services);
        }

        public static void AddServices(this Server server, IEnumerable<ServerServiceDefinition> services)
        {
            foreach (var service in services)
            {
                server.Services.Add(service);
            }
        }

        //Using reflection tricks and assumptions on the way the core gRPC lib works so, if Google changes this, it'll break and only be noticed at runtime :)
        public static Func<TService, ServerServiceDefinition> GetServiceBinder<TService>()
        {
            var serviceType = typeof(TService);
            var baseServiceType = GetBaseType(serviceType);
            var serviceDefinitionType = typeof(ServerServiceDefinition);

            var serviceContainerType = baseServiceType.DeclaringType;
            var methods = serviceContainerType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var binder =
                (from m in methods
                 let parameters = m.GetParameters()
                 where m.Name.Equals("BindService")
                     && parameters.Length == 1
                     && parameters.First().ParameterType.Equals(baseServiceType)
                     && m.ReturnType.Equals(serviceDefinitionType)
                 select m)
            .SingleOrDefault();

            if (binder == null)
            {
                throw new InvalidOperationException($"Could not find service binder for provided service {serviceType.Name}");
            }

            var serviceParameter = Expression.Parameter(serviceType);
            var invocation = Expression.Call(null, binder, new[] { serviceParameter });
            var func = Expression.Lambda<Func<TService, ServerServiceDefinition>>(invocation, false, new[] { serviceParameter }).Compile();
            return func;
        }

        private static Type GetBaseType(Type type)
        {
            var objectType = typeof(object);
            var baseType = type;
            while (!baseType.BaseType.Equals(objectType))
            {
                baseType = baseType.BaseType;
            }
            return baseType;
        }
    }
}
