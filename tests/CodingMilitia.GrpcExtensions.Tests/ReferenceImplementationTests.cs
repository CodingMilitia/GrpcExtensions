using System;
using Xunit;
using Grpc.Core;
using CodingMilitia.GrpcExtensions.Tests.Generated;
using CodingMilitia.GrpcExtensions.Tests.Service;
using System.Threading.Tasks;
using static CodingMilitia.GrpcExtensions.Tests.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.Tests
{
    public class ReferenceImplementationTests : IDisposable
    {
        private readonly Server _server;
        private readonly Channel _channel;
        private readonly SampleServiceClient _client;

        public ReferenceImplementationTests()
        {
            var testRunValues = new TestRunValues();
            _server = new Server
            {
                Services = { BindService(new DumbPipeServiceImplementation(new EchoValueService())) },
                Ports = { new ServerPort(testRunValues.Host, testRunValues.Port, testRunValues.ServerCredentials) }
            };
            _server.Start();
            _channel = new Channel(testRunValues.HostAddress, testRunValues.ClientCredentials);
            _client = new SampleServiceClient(_channel);
        }



        [Fact]
        public async Task ReferenceImplementationWorksAsUsualWithSampleTestService()
        {
            //arrange
            var request = new SampleRequest { Value = 1 };

            //act
            var response = await _client.SendAsync(request);

            //assert
            Assert.Equal(request.Value, response.Value);
        }

        public void Dispose()
        {
            _server.ShutdownAsync().GetAwaiter().GetResult();
            _channel.ShutdownAsync().GetAwaiter().GetResult();
        }
    }
}
