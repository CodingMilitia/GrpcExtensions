using CodingMilitia.GrpcExtensions.Tests.Generated;
using Grpc.Core;
using System.Threading.Tasks;
using static CodingMilitia.GrpcExtensions.Tests.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.Tests.Service
{
    class DumbPipeServiceImplementation : SampleServiceBase
    {
        private readonly IService _service;

        public DumbPipeServiceImplementation(IService service)
        {
            _service = service;
        }

        public override async Task<SampleResponse> Send(SampleRequest request, ServerCallContext context)
        {
            return await _service.SendAsync(request, context.CancellationToken);
        }
    }
}
