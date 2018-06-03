using CodingMilitia.GrpcExtensions.Tests.Generated;
using CodingMilitia.GrpcExtensions.Tests.Service;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static CodingMilitia.GrpcExtensions.Tests.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.Tests
{
    public class PreConfiguredServerFactoryTests : IDisposable
    {
        private readonly Func<IServiceProvider, Server> _serverFactory;
        private readonly Channel _channel;
        private readonly SampleServiceClient _client;

        public PreConfiguredServerFactoryTests()
        {
            var testRunValues = new TestRunValues();
            _serverFactory = _ =>
            {
                return new Server
                {
                    Services = { BindService(new DumbPipeServiceImplementation(new EchoValueService())) },
                    Ports = { new ServerPort(testRunValues.Host, testRunValues.Port, testRunValues.ServerCredentials) }
                };
            };
            _channel = new Channel(testRunValues.HostAddress, testRunValues.ClientCredentials);
            _client = new SampleServiceClient(_channel);
        }

        [Fact]
        public async Task AddingAPreConfiguredServerFactoryHostsAndHandlesRequests()
        {
            //arrange
            var serverHostBuilder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddGrpcServer(_serverFactory);
               });

            var cts = new CancellationTokenSource();
            var serverHostTask = serverHostBuilder.RunConsoleAsync(cts.Token);

            var request = new SampleRequest { Value = 1 };

            //act
            var response = await _client.SendAsync(request);

            //assert
            Assert.Equal(request.Value, response.Value);

            //clean up
            cts.Cancel();
            await serverHostTask;
        }

        public void Dispose()
        {
            _channel.ShutdownAsync().GetAwaiter().GetResult();
        }
    }
}
