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
        private readonly SampleServiceClient _client;

        public ReferenceImplementationTests()
        {
            _server = new Server
            {
                Services = { BindService(new DumbPipeServiceImplementation(new EchoValueService())) },
                Ports = { new ServerPort(TestHelpers.Host, TestHelpers.Port, TestHelpers.Credentials) }
            };
            _server.Start();
            _client = new SampleServiceClient(new Channel(TestHelpers.HostAddress, TestHelpers.ClientCredentials));
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
        }
    }
}
