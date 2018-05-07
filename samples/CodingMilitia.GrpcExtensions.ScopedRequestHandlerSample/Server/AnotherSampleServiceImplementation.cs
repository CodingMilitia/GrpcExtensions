using System.Threading.Tasks;
using CodingMilitia.GrpcExtensions.Hosting;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using static CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Generated.SampleService;

namespace CodingMilitia.GrpcExtensions.ScopedRequestHandlerSample.Server
{
    public class AnotherSampleServiceImplementation : SampleServiceBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AnotherSampleServiceImplementation(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public override async Task<Generated.SampleResponse> Send(Generated.SampleRequest request, ServerCallContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ISampleServiceLogic>();
                var response = await service.SendAsync(request.ToInternalRequest(), context.CancellationToken);
                return response.ToExternalResponse();
            }
        }
    }
}