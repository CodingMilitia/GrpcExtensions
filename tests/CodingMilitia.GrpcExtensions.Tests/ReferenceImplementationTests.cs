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
                Services = { BindService(new EchoValueServiceImplementation()) },
                Ports = { new ServerPort("127.0.0.1", 1234, ServerCredentials.Insecure) }
            };
            _server.Start();
            _client = new SampleServiceClient(new Channel("127.0.0.1:1234", ChannelCredentials.Insecure));
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
