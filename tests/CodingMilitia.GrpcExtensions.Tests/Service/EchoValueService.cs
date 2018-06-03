using CodingMilitia.GrpcExtensions.Tests.Generated;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.GrpcExtensions.Tests.Service
{
    public class EchoValueService : IService
    {
        public Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct)
        {
            return Task.FromResult(new SampleResponse { Value = request.Value });
        }
    }
}