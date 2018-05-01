using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class GrpcBackgroundService : IHostedService
    {
        private readonly Server _server;
        private readonly ILogger<GrpcBackgroundService> _logger;

        public GrpcBackgroundService(Server server, ILogger<GrpcBackgroundService> logger)
        {
            _server = server;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting gRPC server");

            _server.Start();

            _logger.LogDebug(
                "Listening on: {hostingEndpoints}",
                string.Join("; ", _server.Ports.Select(p => $"{p.Host}:{p.BoundPort}"))
            );

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping gRPC server");

            await _server.ShutdownAsync();
            
            _logger.LogDebug("gRPC server stopped");
        }
    }
}