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
    public class AddSimpleServiceImplementationTests : IDisposable
    {
        private readonly TestRunValues _testRunValues;
        private readonly Channel _channel;
        private readonly SampleServiceClient _client;

        public AddSimpleServiceImplementationTests()
        {
            _testRunValues = new TestRunValues();
            _channel = new Channel(_testRunValues.HostAddress, _testRunValues.ClientCredentials);
            _client = new SampleServiceClient(_channel);
        }

        [Fact]
        public async Task AddingASimpleServiceImplementationHostsAndHandlesRequests()
        {
            //arrange
            var serverHostBuilder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddSingleton<IService, EchoValueService>();
                   services.AddGrpcServer<DumbPipeServiceImplementation>(new[] { new ServerPort(_testRunValues.Host, _testRunValues.Port, _testRunValues.ServerCredentials) });
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
