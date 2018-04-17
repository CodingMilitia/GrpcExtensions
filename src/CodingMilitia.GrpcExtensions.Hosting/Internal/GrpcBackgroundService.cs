using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace CodingMilitia.GrpcExtensions.Hosting.Internal
{
    internal class GrpcBackgroundService : IHostedService
    {
        private readonly Server _server;
        
        public GrpcBackgroundService(Server server)
        {
            _server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _server.Start();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.ShutdownAsync();
        }
    }
}