using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class TypedGrpcBackgroundService<TService> : GrpcBackgroundService
    {
        public TypedGrpcBackgroundService(TypedServerContainer<TService> serverContainer, ILogger<GrpcBackgroundService> logger) 
        : base(serverContainer.Server, logger)
        {
        }
    }
}