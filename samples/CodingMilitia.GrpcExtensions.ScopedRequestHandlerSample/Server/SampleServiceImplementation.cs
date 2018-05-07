using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.Hosting;
using Grpc.Core;
using static CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server
{
    public class SampleServiceImplementation : SampleServiceBase
    {
        private readonly IScopedExecutor _scopedExecutor;

        public SampleServiceImplementation(IScopedExecutor scopedExecutor)
        {
            _scopedExecutor = scopedExecutor;
        }

        public override async Task<Generated.SampleResponse> Send(Generated.SampleRequest request, ServerCallContext context)
        {
            return await _scopedExecutor.ExecuteAsync<ISampleServiceLogic, Generated.SampleResponse>(async (service) =>
            {
                var response = await service.SendAsync(request.ToInternalRequest(), context.CancellationToken);
                return response.ToExternalResponse();
            });
        }
    }
}