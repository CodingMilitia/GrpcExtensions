using CodingMilitia.GrpcExtensions.Tests.Generated;
using Grpc.Core;
using System.Threading.Tasks;
using static CodingMilitia.GrpcExtensions.Tests.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.Tests.Service
{
    public class EchoValueServiceImplementation : SampleServiceBase
    {
        public override Task<SampleResponse> Send(SampleRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SampleResponse { Value = request.Value });
        }
    }
}